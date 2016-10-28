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
        public Dictionary<Symbol, ProductionRule> Transitions { get; set; }
        public Dictionary<Symbol, int> PublicTransitions { get; set; }
        public int StateName;
        public bool Explored { get; set; }

        public LR1AutomataState(int stateName, ProductionRule header, List<ProductionRule> contents = null, Dictionary<Symbol, ProductionRule> transitions = null)
        {
            StateName = stateName;
            Header = header;
            Explored = false;
            Contents = contents;
            if (Contents == null)
                Contents = new List<ProductionRule>();

            Transitions = transitions;
            if(Transitions == null)
                Transitions = new Dictionary<Symbol, ProductionRule>();
        }
    }
}
