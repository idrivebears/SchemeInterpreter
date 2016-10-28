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
        public Dictionary<Symbol, ProductionRule> KernelTransitions { get; set; }
        public Dictionary<Symbol, int> PublicTransitions { get; set; }
        public ProductionRule RuleToReduce { get; set; }
        public int StateName;
        public bool Explored { get; set; }

        public LR1AutomataState(int stateName, ProductionRule header, List<ProductionRule> contents = null, Dictionary<Symbol, ProductionRule> kernelTransitions = null)
        {
            StateName = stateName;
            Header = header;
            Explored = false;
            Contents = contents;
            RuleToReduce = null;

            if (Contents == null)
                Contents = new List<ProductionRule>();

            KernelTransitions = kernelTransitions;
            if(KernelTransitions == null)
                KernelTransitions = new Dictionary<Symbol, ProductionRule>();
        }
    }
}
