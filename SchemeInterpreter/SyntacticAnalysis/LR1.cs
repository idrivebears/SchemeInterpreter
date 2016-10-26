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

        // Add handling of end of string
        private void BuildAutomata()
        {
            var automata = new Dictionary<ProductionRule, LR1AutomataState>();

            //Start with first states
            var firstState = new LR1AutomataState(0, _grammar.ProductionRules.First(), new List<ProductionRule>());
            firstState.Contents.Add(firstState.Header);
            automata.Add(firstState.Header, firstState);

            while (automata.Any(s => s.Value.Explored == false))
            {
                var state = automata.First(s => s.Value.Explored == false);

                // First step is to generate state contents
                foreach (var rule in state.Value.Contents)
                {
                    if (rule.Caret != rule.Body.Count)
                    {
                        var symbolRead = rule.Body.ElementAt(rule.Caret + 1);
                        if (symbolRead.IsNonTerminal())
                        {
                            // Add all productions of symbol, unrepeated
                            var foundOcurrances = _grammar.ProductionRules.FindAll(r => r.Header == symbolRead);
                            foreach (var occurrance in foundOcurrances)
                            {
                                if (!state.Value.Contents.Contains(occurrance))
                                    state.Value.Contents.Add(occurrance);
                            }
                        }
                    }
                }

                // Check each rule in the state, generating or linking to the transition states
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

                state.Value.Explored = true;
            }
        }
    }
}
