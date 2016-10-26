using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.LexerEngine;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter.SyntacticAnalysis
{
    //Left at: create new state into automata
    public class LR1
    {

        Grammar _grammar;

        public LR1(Grammar g)
        {
            _grammar = g;
            BuildAutomata();
        }

        private void BuildAutomata()
        {
            var automata = new List<LR1AutomataState>();

            //Start with first state
            var firstState = new LR1AutomataState(0, _grammar.ProductionRules.First(), _grammar.ProductionRules);
            automata.Add(firstState);

            foreach (var state in automata)
            {
                if (!state.Explored)
                {
                    foreach (var rule in state.Contents)
                    {
                        //If caret is not at the end of the body
                        if (rule.Caret != rule.Body.Count)
                        {
                            //Get transition symbol
                            rule.Body.ElementAt(rule.Caret);
                            // Generate new state with rule as header only if state
                            // doesnt previously exist

                            var alreadyExists = false;
                            foreach (var checkState in automata)
                                if (checkState.Header.Equals(rule))
                                    alreadyExists = true;

                            if (!alreadyExists)
                            {
                                rule.Caret++;
                                //automata.Add(new LR1AutomataState(state.StateName++, rule,));
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
        }
    }
}
