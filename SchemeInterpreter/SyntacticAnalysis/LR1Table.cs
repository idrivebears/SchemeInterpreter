using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.LexerEngine;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter.SyntacticAnalysis
{
    class LR1Table
    {
        public enum ActionTypes {Shift, Reduce};

        private readonly Grammar _grammar;
        private readonly Dictionary<Symbol, int> _terminalLookup;
        private readonly Dictionary<Tuple<Symbol, int>, int> _gotoLookup;
 
        private class Action
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
            public string TokenClass { get; private set; }
            public ExtendedSymbol(SymTypes type, string val, string tokenClass) : base(type, val)
            {
                TokenClass = tokenClass;
            }
        }

        private readonly Action[,] _table;

        public LR1Table(Grammar g, LR1 automata)
        {
            _grammar = g; //consider building a new grammar, not just stealing the pointer (as LL1).
            _terminalLookup = new Dictionary<Symbol, int>();
            _gotoLookup = new Dictionary<Tuple<Symbol, int>, int>();

            //Get terminal symbols from grammar
            var terminals = (_grammar.Symbols.Where(x => x.IsTerminal())).ToArray();
            var maxState = automata.AutomataStates.Keys.Count;

            for(var i=0; i<terminals.Length;i++)
                _terminalLookup.Add(terminals[i], i);
            _terminalLookup.Add(new Symbol(Symbol.SymTypes.EOS, "$"), terminals.Length); //add the end of string symbol


            _table = new Action[_terminalLookup.Count, maxState+1]; //generate Action lookUp table

            //Construct the table with the provided automaton


        }

        public bool Accept(string input)
        {
            var stateStack = new Stack<int>();
            var symbolStack = new Stack<Symbol>();
            var inputQueue = new Queue<ExtendedSymbol>();

            //build input queue
            var lexer = LexerGenerator.Generate("entrega.miniflex"); //Rembebr to change lexer
            var tokens = lexer.Tokenize(input);
            foreach (var token in tokens)
                inputQueue.Enqueue(new ExtendedSymbol(Symbol.SymTypes.Terminal, token.Value, token.Type));

            //Initialize stacks
            stateStack.Push(0); //state 0 is init state

            while (stateStack.Peek() != 1)
            {
                var focusState = stateStack.Peek();
                var focusSym = inputQueue.Peek();

                var focusAction = _table[focusState, _terminalLookup[focusSym]];

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
                            inputQueue.Dequeue();
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
