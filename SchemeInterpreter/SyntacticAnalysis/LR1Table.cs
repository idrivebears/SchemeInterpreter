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
        public enum ActionTypes {Shift, Reduce};

        private readonly Grammar _grammar;
        private readonly Dictionary<Symbol, int> _terminalLookup;
        private readonly Dictionary<Tuple<Symbol, int>, int> _gotoLookup;
        private readonly LR1 _lr1;
        private readonly int _acceptanceState;

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

        private readonly Action[,] _table;

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

            _acceptanceState = _lr1.AutomataStates.Keys.Count-1;
            var maxState = _acceptanceState;

            for(var i=0; i<terminals.Length;i++)
                _terminalLookup.Add(terminals[i], i);
            _terminalLookup.Add(new Symbol(Symbol.SymTypes.EOS, "$"), terminals.Length); //add the end of string symbol


            _table = new Action[_terminalLookup.Count, maxState+1]; //generate Action lookUp table

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

            //build input queue
            var lexer = LexerGenerator.Generate("LR.miniflex"); //Rembebr to change lexer
            var tokens = lexer.Tokenize(input);
            foreach (var token in tokens)
                if (token.Type != "(end)" && token.Type != "(white-space)")
                    inputQueue.Enqueue(token.Type == "(id)"
                        ? new ExtendedSymbol(Symbol.SymTypes.Terminal, "id", token.Value)
                        : new ExtendedSymbol(Symbol.SymTypes.Terminal, token.Value, token.Type));

            inputQueue.Enqueue(new ExtendedSymbol(Symbol.SymTypes.EOS, "$", "EoS"));
            //Initialize stacks
            stateStack.Push(0); //state 0 is init state

            while (stateStack.Peek() != _acceptanceState || inputQueue.Count > 1)
            {
                var focusState = stateStack.Peek();
                var focusSym = inputQueue.Peek();

                var focusAction = _table[_terminalLookup[focusSym], focusState];

                if (focusAction == null)
                    return false; //Action is not defined for the current state and terminal.

                switch (focusAction.Type)
                {
                    case ActionTypes.Shift:
                        //Do the shift -> 
                        symbolStack.Push(inputQueue.Dequeue()); //Consume the symbol and push it to the symStack
                        stateStack.Push(focusAction.ActionVal); //push the next symbol
                        break;
                    case ActionTypes.Reduce:
                        //Do the Reduce
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

            return true;
        }


    }
}
