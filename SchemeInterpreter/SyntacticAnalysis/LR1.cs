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

        /*
         Add eos
         */
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
                var rulesToCheck = new Queue<ProductionRule>(state.Value.Contents.ToList());

                while (rulesToCheck.Count > 0)
                {
                    var currentRule = rulesToCheck.Dequeue();
                    if (currentRule.Caret < currentRule.Body.Count)
                    {
                        var symbolRead = currentRule.Body.ElementAt(currentRule.Caret);
                        if (symbolRead.IsNonTerminal())
                        {
                            // Add all productions of symbol, unrepeated
                            var foundOcurrances = _grammar.ProductionRules.FindAll(r => r.Header == symbolRead);
                            foreach (var occurrance in foundOcurrances)
                            {
                                if (!state.Value.Contents.Contains(occurrance))
                                {
                                    if (!occurrance.Body.Any(s => s.IsEpsilon()))
                                    {
                                        rulesToCheck.Enqueue(occurrance);
                                        state.Value.Contents.Add(occurrance);
                                    }
                                    else
                                    {
                                        var tempNewRule = new ProductionRule(occurrance);
                                        tempNewRule.Caret++;
                                        state.Value.Contents.Add(tempNewRule);
                                        state.Value.RuleToReduce = tempNewRule;
                                    }
                                }
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
                        if (state.Value.KernelTransitions.ContainsKey(readSymbol))
                        {
                            var nextStateHeader = state.Value.KernelTransitions[readSymbol];
                            if (!_automata[nextStateHeader].Contents.Contains(newRule))
                            {
                                _automata[nextStateHeader].Contents.Add(newRule);
                            }
                        }
                        // Transition doesnt exist
                        else
                        {
                            // If state already exists
                            if (_automata.ContainsKey(newRule))
                            {
                                // Add transition to the state
                                state.Value.KernelTransitions.Add(readSymbol, newRule);
                            }
                            // State doesnt exist
                            else
                            {
                                var newStateName = state.Value.StateName + 1;
                                _automata.Add(newRule, new LR1AutomataState(newStateName, newRule));
                                _automata[newRule].Contents.Add(newRule);

                                // add transition
                                state.Value.KernelTransitions.Add(readSymbol, newRule);
                            }
                        }
                    }
                }

                /*if (state.Value.Contents.Count == 1 && state.Value.KernelTransitions.Count == 0)
                {
                    state.Value.RuleToReduce = state.Value.Contents[0];
                }*/


                state.Value.Explored = true;
            }


            // Add acceptance state
            var accSym = new Symbol(Symbol.SymTypes.EOS, "$");
            var intermState = _automata.ElementAt(1);
            var acceptanceState = new LR1AutomataState(_automata.Keys.Count, new ProductionRule(accSym, new List<Symbol>()));
            intermState.Value.KernelTransitions.Add(accSym, acceptanceState.Header);
            _automata.Add(acceptanceState.Header, acceptanceState);
            //_automata[firstState.Header].KernelTransitions.Add(firstState.Header.Header, acceptanceState.Header);

            //Add the rule to reduce for each state
            foreach (var s in _automata)
            {
                var state = s.Value;
                if (state.Contents.Any(x => x.Caret == x.Body.Count))
                {
                    var r = state.Contents.First(x => x.Caret == x.Body.Count);
                    state.RuleToReduce = r;
                }
            }

        }

        private void GenerateAutomataStates()
        {
            AutomataStates = new Dictionary<int, LR1AutomataState>();
            int uniqueID = 0;

            for (var i = 0; i < _automata.Values.Count; i++)
            {
                var lr1AutomataState = _automata.Values.ToArray()[i];
                lr1AutomataState.StateName = uniqueID;
                AutomataStates.Add(uniqueID++, lr1AutomataState);
            }

            // Generate public transitions, accesible by number
            foreach (var lr1AutomataState in _automata.Values)
            {
                lr1AutomataState.PublicTransitions = new Dictionary<Symbol, int>();

                foreach (var transition in lr1AutomataState.KernelTransitions)
                {

                    var state = _automata[transition.Value];
                    var stateIndex = AutomataStates.First(x => x.Value.Header == state.Header).Key;
                    lr1AutomataState.PublicTransitions.Add(transition.Key, stateIndex);
                }

            }

            //AutomataStates[0].PublicTransitions[AutomataStates[0].Header.Header] = AutomataStates.Count - 1;

        }

    }
}
