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
            var automata = new Dictionary<ProductionRule, LR1AutomataState>();

            //Start with first statezs
            var firstState = new LR1AutomataState(0, _grammar.ProductionRules.First(), _grammar.ProductionRules);
            automata.Add(firstState.Header, firstState);

            while (automata.Any(s => s.Value.Explored == false))
            {
                var state = automata.First(s => s.Value.Explored == false);

                foreach (var rule in state.Value.Contents)
                {
                    //If caret is not at the end of the body
                    if (rule.Caret != rule.Body.Count)
                    {
                        // Read transition symbol
                        var readSymbol = rule.Body.ElementAt(rule.Caret);
                        rule.Caret++;

                        // Generate new state with rule as header only if state doesnt previously exist
                        // Transition already exists, therefore state also exists
                        if (state.Value.Transitions.ContainsKey(readSymbol))
                        {
                            var nextStateHeader = state.Value.Transitions[readSymbol];
                            automata[nextStateHeader].Contents.Add(rule);
                        }
                        // Transition doesnt exist
                        else
                        {
                            // If state already exists
                            if (automata.ContainsKey(rule))
                            {
                                // Add transition to the state
                                state.Value.Transitions.Add(readSymbol, rule);
                            }
                            // State doesnt exist
                            else
                            {
                                var newStateName = state.Value.StateName + 1;
                                automata.Add(rule, new LR1AutomataState(newStateName, rule, new List<ProductionRule>()));
                                automata[rule].Contents.Add(rule);

                                // add transition
                                state.Value.Transitions.Add(readSymbol, rule);
                            }
                        }
                    }
                }
            }
        }
    }
}
