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
        private readonly Symbol[] _symbLookup;
        private ProductionRule _start;

        public LL1(Grammar g)
        {
            //Get all terminal symbols from Grammar
            _symbLookup = (g.Symbols.Where(s => s.IsTerminal())).ToArray();
            _table = new int[_symbLookup.Length,g.ProductionRules.Count];
            
            //Get start production (first head of production)
            _start = g.ProductionRules[0];

            //Generate table

            //FIRST
            g.GenerateFirstSets();
            var first = g.FirstSets;

            //FOLLOW
            //g.GenerateFollowSets();
            //var follow = g.FollowSets;

            for (var i = 0; i < g.ProductionRules.Count; i++)
            {
                
            }
        }

        public bool Accept(string input)
        {
            var sym = new Stack<Symbol>();
            sym.Push(new Symbol(Symbol.SymTypes.Terminal, "$"));

            //ToDo Acceptance algorithm

            return false;
        }
    }
}
