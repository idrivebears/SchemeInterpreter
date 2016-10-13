using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter.SyntacticAnalysis
{
    class LL1
    {
        private int[,] _table;
        private readonly Dictionary<Symbol, int> _symbLookup;
        private Symbol _start;

        public LL1(Grammar g)
        {
            //Get all terminal symbols from Grammar
            var terminals = (g.Symbols.Where(s => s.IsTerminal())).ToArray();
            for (var i = 0; i < terminals.Length; i++)
            {
                _symbLookup.Add(terminals[i], i);
            }

            _table = new int[terminals.Length,g.ProductionRules.Count];
            
            //Get start production (first head of production)
            _start = g.ProductionRules[0].Header;

            //Generate table
            //Ensure First  & follow sets
            g.GenerateFirstSets();
            //g.GenerateFollowSets();

            for (var i = 0; i < g.ProductionRules.Count; i++)
            {
                var focusFirst = g.GetFirstSet(i);
                //check for epsilon set

                if (focusFirst.Any(s => s.IsEpsilon()))
                {
                    //handle FOLLOW, set content is a single symbol EPSILON
                }
                
                foreach (var term in focusFirst)
                {
                    //get index
                    var symIndex = _symbLookup[term];
                    //place production rule id in table
                    _table[symIndex, i] = i;
                }
            }
        }

        public bool Accept(string input)
        {
            var sym = new Stack<Symbol>();
            sym.Push(new Symbol(Symbol.SymTypes.Terminal, "$"));
            sym.Push(_start); //initialize eval stack

            //ToDo Acceptance algorithm

            return false;
        }
    }
}
