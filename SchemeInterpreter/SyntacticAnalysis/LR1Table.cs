using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.LexerEngine;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter.SyntacticAnalysis
{
    public class LR1Table
    {
        public enum ActionTypes { Shift, Reduce };

        private readonly Grammar _grammar;
        private readonly Action[,] _table;
        private readonly Dictionary<Symbol, int> _terminalLookup;
        private readonly Dictionary<Tuple<Symbol, int>, int> _gotoLookup;
        private List<Tuple<int, Symbol>> _errorList;
        private readonly LR1 _lr1;
        private readonly int _acceptanceState;
        private int inputLength;

        internal class Action
        {
            public readonly ActionTypes Type;
            public readonly int ActionVal;

            public Action(ActionTypes type, int actionVal)
            {
                Type = type;
                ActionVal = actionVal;
            }
        }

        private class ExtendedSymbol : Symbol
        {
            private string _tokenClass;
            public ExtendedSymbol(SymTypes type, string val, string tokenClass) : base(type, val)
            {
                _tokenClass = tokenClass;
            }
        }



        public LR1Table(Grammar g)
        {
            _grammar = g; //consider building a new grammar, not just stealing the pointer (as LL1).
            _grammar.GenerateFirstAndFollow();
            _terminalLookup = new Dictionary<Symbol, int>();
            _gotoLookup = new Dictionary<Tuple<Symbol, int>, int>();
            _lr1 = new LR1(g);


            //Get terminal symbols from grammar
            var terminals = (_grammar.Symbols.Where(x => x.IsTerminal())).ToArray();
            var nonTerminals = (_grammar.Symbols.Where(x => x.IsNonTerminal())).ToArray();

            _acceptanceState = _lr1.AutomataStates.Keys.Count - 1;
            var maxState = _acceptanceState;

            for (var i = 0; i < terminals.Length; i++)
                _terminalLookup.Add(terminals[i], i);
            _terminalLookup.Add(new Symbol(Symbol.SymTypes.EOS, "$"), terminals.Length); //add the end of string symbol


            _table = new Action[_terminalLookup.Count, maxState + 1]; //generate Action lookUp table

            //Construct the table with the provided automaton
            for (var i = 0; i < maxState; i++)
            {
                var focusState = _lr1.AutomataStates[i]; //get the nth state

                //set reduce Actions
                var reduceRule = focusState.RuleToReduce;

                if (reduceRule != null)
                {
                    reduceRule.Caret = 0; //reset the caret
                    for (var j = 0; j < _grammar.ProductionRules.Count; j++)
                    {
                        if (!_grammar.ProductionRules[j].Equals(reduceRule)) continue;
                        foreach (var follow in g.FollowSets[focusState.Header.Header])
                            _table[_terminalLookup[follow], focusState.StateName] = new Action(ActionTypes.Reduce, j);
                        break;
                    }
                }
                //set shift Actions

                foreach (var term in _terminalLookup.Keys.Where(term => focusState.PublicTransitions.ContainsKey(term)))
                    _table[_terminalLookup[term], focusState.StateName] = new Action(ActionTypes.Shift, focusState.PublicTransitions[term]);

                //set goto lookup
                foreach (var nonTerm in nonTerminals.Where(nonTerm => focusState.PublicTransitions.ContainsKey(nonTerm)))
                    _gotoLookup[new Tuple<Symbol, int>(nonTerm, focusState.StateName)] =
                        focusState.PublicTransitions[nonTerm];
            }
        }

        public bool Accept(string input)
        {
            var stateStack = new Stack<int>();
            var symbolStack = new Stack<Symbol>();
            var inputQueue = new Queue<ExtendedSymbol>();
            _errorList = new List<Tuple<int, Symbol>>();
            //build input queue
            var lexer = LexerGenerator.Generate("Scheme.miniflex"); //Rembebr to change lexer
            var tokens = lexer.Tokenize(input);
            foreach (var token in tokens)
                if (token.Type != "(end)" && token.Type != "(White-space)")
                    inputQueue.Enqueue(new ExtendedSymbol(Symbol.SymTypes.Terminal, token.Type, token.Value));

            inputQueue.Enqueue(new ExtendedSymbol(Symbol.SymTypes.EOS, "$", "EoS"));
            inputLength = inputQueue.Count;
            //Initialize stacks
            stateStack.Push(0); //state 0 is init state

            while (stateStack.Peek() != _acceptanceState)
            {

                var focusState = stateStack.Peek();
                var focusSym = inputQueue.Peek();

                var focusAction = _table[_terminalLookup[focusSym], focusState];

                PrintDebug(stateStack, symbolStack, inputQueue, focusAction);

                if (focusAction == null)
                {
                    var gotoFound = false;
                    /*foreach (var i in _grammar.Symbols)
                    { 
                        var y = _gotoLookup.Any(x => x.Key.Item1 == i && x.Key.Item2 == focusState);
                        if (_gotoLookup.Any(x => x.Key.Item1 == i && x.Key.Item2 == focusState))
                        {
                            //Goto exists
                            var gotoElement = _gotoLookup.First(x => x.Key.Item1 == i && x.Key.Item2 == focusState);
                            stateStack.Push(gotoElement.Value);
                            symbolStack.Push(i);
                            _errorList.Add(new Tuple<int, Symbol>(inputQueue.Count, i));
                            gotoFound = true;
                            break;
                        }
                    }
                    if (!gotoFound)
                    {
                        stateStack.Pop();
                        if (stateStack.Count == 0) return false;
                    }*/
                    //continue;
                    return false; //Action is not defined for the current state and terminal.
                }
                switch (focusAction.Type)
                {
                    case ActionTypes.Shift:
                        //Do the shift -> 
                        symbolStack.Push(inputQueue.Dequeue()); //Consume the symbol and push it to the symStack
                        stateStack.Push(focusAction.ActionVal); //push the next symbol
                        break;
                    case ActionTypes.Reduce:
                        //Do the Reduce
                        var debugProduction = _grammar.ProductionRules[focusAction.ActionVal];
                        //Get production body size
                        var productionSize =
                            _grammar.ProductionRules[focusAction.ActionVal].Body.Count(x => !x.IsEpsilon());

                        for (var i = 0; i < productionSize; i++)
                        {
                            stateStack.Pop(); //pop n elements, where n is the size of the production body
                            symbolStack.Pop();
                        }
                        symbolStack.Push(_grammar.ProductionRules[focusAction.ActionVal].Header); //push production header
                        //Goto next state
                        stateStack.Push(_gotoLookup[new Tuple<Symbol, int>(symbolStack.Peek(), stateStack.Peek())]);
                        break;
                }
            }
            PrintErrors();
            return true;
        }

        private static void PrintDebug(IEnumerable<int> stateStack, IEnumerable<Symbol> symStack, IEnumerable<ExtendedSymbol> inputQueue, Action action)
        {
            Console.Write("Estados> ");
            foreach (var state in stateStack)
                Console.Write("{0} | ", state);

            Console.WriteLine("");
            Console.Write("Simbolos> ");
            foreach (var sym in symStack)
                Console.Write("{0} ", sym.Value);

            Console.WriteLine("");
            Console.Write("Entrada> ");
            foreach (var sym in inputQueue)
                Console.Write("{0} ", sym.Value);

            if (action == null)
                return;
            Console.WriteLine("");
            Console.Write("Accion> ");
            Console.WriteLine("{0} :: {1}", action.Type, action.ActionVal);

            Console.WriteLine("");
        }
        private void PrintErrors()
        {
            if (_errorList.Count == 0) Console.WriteLine("No sintax error found.");
            foreach (var i in _errorList)
            {
                Console.WriteLine("Sintax error on: " + (inputLength - i.Item1) + ", expecting: " + i.Item2.ToString());
            }
        }

    }
}