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

        private Grammar _grammar;
        public Dictionary<ProductionRule, LR1AutomataState> _automata { get; private set; }
        public Dictionary<int, LR1AutomataState> AutomataStates { get; private set; }

    public LR1(Grammar g)
        {
            _grammar = g;
            BuildAutomata();
            GenerateAutomataStates();
        }

        // Add handling of end of string
        private void BuildAutomata()
        {
            _automata = new Dictionary<ProductionRule, LR1AutomataState>();

            //Start with first states
            var firstState = new LR1AutomataState(0, _grammar.ProductionRules.First());
            firstState.Contents.Add(firstState.Header);
            _automata.Add(firstState.Header, firstState);

            while (_automata.Any(s => s.Value.Explored == false))
            {
                var state = _automata.First(s => s.Value.Explored == false);

                // First step is to generate state contents
                // **.ToList() is used to make a copy of the List, not very efficient, should
                // be done somehow else
                foreach (var rule in state.Value.Contents.ToList())
                {
                    if (rule.Caret < rule.Body.Count)
                    {
                        var symbolRead = rule.Body.ElementAt(rule.Caret);
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
                        var newRule = new ProductionRule(rule);
                        newRule.Caret++;
                        

                        // Generate new state with rule as header only if state doesnt previously exist
                        // Transition already exists, therefore state also exists
                        if (state.Value.Transitions.ContainsKey(readSymbol))
                        {
                            var nextStateHeader = state.Value.Transitions[readSymbol];
                            _automata[nextStateHeader].Contents.Add(newRule);
                        }
                        // Transition doesnt exist
                        else
                        {
                            // If state already exists
                            if (_automata.ContainsKey(newRule))
                            {
                                // Add transition to the state
                                state.Value.Transitions.Add(readSymbol, newRule);
                            }
                            // State doesnt exist
                            else
                            {
                                var newStateName = state.Value.StateName + 1;
                                _automata.Add(newRule, new LR1AutomataState(newStateName, newRule));
                                _automata[newRule].Contents.Add(newRule);

                                // add transition
                                state.Value.Transitions.Add(readSymbol, newRule);
                            }
                        }
                    }
                }

                state.Value.Explored = true;
            }
        }

        private void GenerateAutomataStates()
        {
            AutomataStates = new Dictionary<int, LR1AutomataState>();
            int uniqueID = 0;

            foreach (var lr1AutomataState in _automata.Values)
            {
                lr1AutomataState.StateName = uniqueID;
                AutomataStates.Add(uniqueID++, lr1AutomataState);
            }
        }

    }
}
