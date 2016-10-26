using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.LexerEngine;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter.SyntacticAnalysis
{
    public class LR1AutomataState
    {
        public ProductionRule Header;
        public List<ProductionRule> Contents { get; set; }
        public Dictionary<Symbol, int> Transitions { get; set; }
        public int StateName;
        public  bool Explored = false;

        public LR1AutomataState(int stateName, ProductionRule header, List<ProductionRule> contents)
        {
            StateName = stateName;
            Header = header;
            Contents = contents;
        }
    }
}
