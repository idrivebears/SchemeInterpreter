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
            var automata = new Dictionary<int, LR1AutomataState>();

            //Start with first statezs
            var firstState = new LR1AutomataState(0, _grammar.ProductionRules.First(), _grammar.ProductionRules);
            automata.Add(firstState.StateName, firstState);

            
            while (automata.Any(s => s.Value.Explored == false))
            {
                var state = automata.First(s => s.Value.Explored == false).Value;

                foreach (var rule in state.Contents)
                {
                    //If caret is not at the end of the body
                    if (rule.Caret != rule.Body.Count)
                    {
                        // Read transition symbol
                        var readSymbol = rule.Body.ElementAt(rule.Caret);
                        rule.Caret++;

                        // Generate new state with rule as header only if state doesnt previously exist
                        // Transition already exists, therefore state also exists
                        if (state.Transitions.ContainsKey(readSymbol))
                        {

                        }
                        // Transition doesnt exist
                        else
                        {
                            // If state already exists
                            if (automata.Any(s => s.Value.Header.Equals(rule)))
                            {
                                automata[state.StateName].Contents.Add(rule);
                                //automata[state.StateName].Transitions.Add();
                            }
                            // State doesnt exist
                            else
                            {
                                var newStateName = state.StateName + 1;
                                automata.Add(newStateName, new LR1AutomataState(newStateName, rule, new List<ProductionRule>()));
                                automata[newStateName].Contents.Add(rule);
                            }
                        }
                    }
                }
            }
        }
    }
}
