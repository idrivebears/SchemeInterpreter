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
        private readonly LexerEngine.Lexer lexer;
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

        public struct State
        {
            public readonly int StateId;
            public object Result;
            public readonly Symbol Primary;

            public State(int id)
            {
                StateId = id;
                Result = null;
                Primary = null;
            }

            public State(int id, object result)
            {
                StateId = id;
                Result = result;
                Primary = null;
            }

            public State(int id, Symbol primary)
            {
                StateId = id;
                Result = null;
                Primary = primary;
            }

            public override string ToString()
            {
                if (Result != null)
                    return @"<" + StateId + "> " + Result;
                return StateId.ToString();
            }
        }

        public class ExtendedSymbol : Symbol
        {
            private string _tokenClass;
            public string Value;
            public ExtendedSymbol(SymTypes type, string val, string tokenClass) : base(type, tokenClass)
            {
                _tokenClass = tokenClass;
                Value = val;
            }
        }



        public LR1Table(Grammar g, string miniflex)
        {

            lexer = LexerGenerator.Generate(miniflex);
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
            var stateStack = new Stack<State>();
            var symbolStack = new Stack<Symbol>();
            var inputQueue = new Queue<ExtendedSymbol>();
            _errorList = new List<Tuple<int, Symbol>>();

            //build input queue
            var tokens = lexer.Tokenize(input);
            foreach (var token in tokens)
                if (token.Type != "(end)" && token.Type != "(White-space)")
                    inputQueue.Enqueue(new ExtendedSymbol(Symbol.SymTypes.Terminal, token.Value, token.Type));

            inputQueue.Enqueue(new ExtendedSymbol(Symbol.SymTypes.EOS, "EoS", "$"));
            inputLength = inputQueue.Count;
            //Initialize stacks
            stateStack.Push(new State(0)); //state 0 is init state

            while (stateStack.Peek().StateId != _acceptanceState)
            {

                var focusState = stateStack.Peek();
                var focusSym = inputQueue.Peek();

                var focusAction = _table[_terminalLookup[focusSym], focusState.StateId];

                PrintDebug(stateStack, symbolStack, inputQueue, focusAction);

                if (focusAction == null)
                {
                    var gotoFound = false;
                    foreach (var i in _grammar.Symbols)
                    { 
                        var y = _gotoLookup.Any(x => x.Key.Item1 == i && x.Key.Item2 == focusState.StateId);
                        if (_gotoLookup.Any(x => x.Key.Item1 == i && x.Key.Item2 == focusState.StateId))
                        {
                            //Goto exists
                            var gotoElement = _gotoLookup.First(x => x.Key.Item1 == i && x.Key.Item2 == focusState.StateId);
                            stateStack.Push(new State(gotoElement.Value));
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
                    }
                    continue;
                    //return false; //Action is not defined for the current state and terminal.
                }
                switch (focusAction.Type)
                {
                    case ActionTypes.Shift:
                        //Do the shift -> 
                        var topsym = inputQueue.Dequeue();
                        symbolStack.Push(topsym); //Consume the symbol and push it to the symStack
                        stateStack.Push(new State(focusAction.ActionVal, topsym)); //Generate the symState (leaf state) with the symbol
                        break;
                    case ActionTypes.Reduce:
                        //Do the Reduce
                        var production = _grammar.ProductionRules[focusAction.ActionVal];
                        //Get production body size
                        var productionSize =
                            _grammar.ProductionRules[focusAction.ActionVal].Body.Count(x => !x.IsEpsilon());

                        var stateArgs = new List<State>();

                        for (var i = 0; i < productionSize; i++)
                        {
                            stateArgs.Add(stateStack.Pop());  //Catch children states
                            symbolStack.Pop(); //DONT Catch symbols (handled on states on shift)
                        }
                        //EXECUTE THE ACTION
                        var result = production.SemanticAction(stateArgs);

                        symbolStack.Push(_grammar.ProductionRules[focusAction.ActionVal].Header); //push production header
                        //Goto next state
                        stateStack.Push(new State(_gotoLookup[new Tuple<Symbol, int>(symbolStack.Peek(), stateStack.Peek().StateId)], result));
                        break;
                }
            }
            PrintErrors();

            return true;
        }

        private static void PrintDebug(IEnumerable<State> stateStack, IEnumerable<Symbol> symStack, IEnumerable<ExtendedSymbol> inputQueue, Action action)
        {
            Console.Write("Estados> ");
            foreach (var state in stateStack)
                Console.Write("{0} | ", state);

            Console.WriteLine("");
            Console.Write("Simbolos> ");
            foreach (var sym in symStack)
                Console.Write("{0} ", sym.TokenClass);

            Console.WriteLine("");
            Console.Write("Entrada> ");
            foreach (var sym in inputQueue)
                Console.Write("{0} ", sym.TokenClass);

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