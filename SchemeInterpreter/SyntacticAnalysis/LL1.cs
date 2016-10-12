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

        public LL1(Grammar g)
        {
            //Get all terminal symbols from Grammar
            _symbLookup = (g.Symbols.Where(s => s.IsTerminal())).ToArray();
            _table = new int[_symbLookup.Length,g.ProductionRules.Count];

            //ToDo: Generate content for table using FIRST and FOLLOW sets
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
