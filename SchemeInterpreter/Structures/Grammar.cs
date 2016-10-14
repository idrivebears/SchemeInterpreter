using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Structures
{
    public class Grammar
    {
        public readonly List<ProductionRule> ProductionRules;
        public readonly List<Symbol> Symbols;
        public Dictionary<Symbol, HashSet<Symbol>> FirstSets;
        public Dictionary<Symbol, HashSet<Symbol>> FollowSets;

        public Grammar(IEnumerable<ProductionRule> productionRules, IEnumerable<Symbol> symbols)
        {
            ProductionRules = new List<ProductionRule>(productionRules);
            Symbols = new List<Symbol>(symbols);
        }

        public void GenerateFollowSets()
        {
            // Place $ symbol on initial symbol's followset
            // Start with initial symbol:
            //      look for symbol in all productions
            //      if found, check next symbol
            //          if symbol is terminal, add to followset
            //          if symbol is non-terminal add the follow of that symbol
            // repeat until all follows are calculated
            
            FollowSets = new Dictionary<Symbol, HashSet<Symbol>>();

            foreach (var sym in Symbols)
                FollowSets[sym] = new HashSet<Symbol>();

            var initialRule = ProductionRules.First();
            FollowSets[initialRule.Header].Add(new Symbol(Symbol.SymTypes.EOS, "$"));

            // Check all productions
            foreach(var header in ProductionRules)
            {
                var currentHeader = header.Header;
                // Look for header in ProductionRule

                foreach (var rule in ProductionRules)
                {
                    if (rule.Body.Contains(currentHeader) && rule.Header != currentHeader)
                    {
                        //Check next element in line
                        var occurance = rule.Body.IndexOf(currentHeader);
                        if (occurance + 1 < rule.Body.Count)
                        {
                            var next = rule.Body.ElementAt(occurance + 1);

                            // Check if first is an epsilon
                            if (FirstSets[next].Any(s => s.IsEpsilon()))
                            {
                                //Contains epsilon, we should add the follow of the Header
                                FollowSets[currentHeader].UnionWith(FollowSets[rule.Header]);
                                FollowSets[currentHeader].UnionWith(FirstSets[next]);
                            }
                            else
                            {
                                // If its another production rule or terminal symbol, add the First(x) element to the FollowSet
                                FollowSets[currentHeader].UnionWith(FirstSets[next]);
                            }
                        }
                        // The match was found at the end of the production, same case as finding and EPSILON
                        else {
                            FollowSets[currentHeader].UnionWith(FollowSets[rule.Header]);
                            
                        }
                        //Remove epsilons from FollowSets
                        FollowSets[currentHeader].RemoveWhere(s => s.IsEpsilon());
                    }
                }

            }
        }

        public void GenerateFollowSets2()
        {
            /*
             * To compute FOLLOW(A) for all nonterminals A, apply the following rules
                until nothing can be added to any FOLLOW set.
                1. Place $ in FOLLOW(S), where S is the start symbol, and $ is the input
                    right endmarker.
                2. If there is a production A -+ aBC, then everything in FIRST(C) except EPSILON
                    is in FOLLOW(B).
                3. If there is a production A -+ aB, or a production A -> aBc, where
                    FIRST(c) contains EPSILON, then everything in FOLLOW(A) is in FOLLOW(B).
             */
            
            FollowSets = new Dictionary<Symbol, HashSet<Symbol>>();

            foreach (var symbol in Symbols)
            {
                FollowSets.Add(symbol, new HashSet<Symbol>());
            }

            // Add $ in FOLLOW(S)

            var initialRule = ProductionRules.First();
            FollowSets[initialRule.Header].Add(new Symbol(Symbol.SymTypes.EOS, "$"));

            // Apply rule 2
            foreach (var productionRule in ProductionRules)
            {
                for (int i = 0; i < productionRule.Body.Count - 1; i++)
                {
                    var currentSymbol = productionRule.Body[i];
                    var nextSymbol = productionRule.Body[i + 1];
                    FollowSets[currentSymbol].UnionWith(FirstSets[nextSymbol]);
                    FollowSets[currentSymbol].RemoveWhere(s => s.IsEpsilon());
                }
            }
           
            var changeHasOccurred = true;
            while (changeHasOccurred)
            {
                changeHasOccurred = false;

                foreach (var productionRule in ProductionRules)
                {
              
                    for (int i = productionRule.Body.Count - 1; i >= 0; i--)
                    {
                        var setOfBeta = FollowSets[productionRule.Body[i]];
                        var setToAdd = FollowSets[productionRule.Header];

                        if (!setOfBeta.IsSubsetOf(setToAdd))
                            changeHasOccurred = true;

                        setOfBeta.UnionWith(setToAdd);

                        if (!FirstSets[productionRule.Body[i]].Any(e => e.IsEpsilon())) ;
                            break;
                    }   
                }

            }
        }

        public void GenerateFirstSets()
        {
            // Initialize empty sets and terminal symbols
            FirstSets = new Dictionary<Symbol, HashSet<Symbol>>();

            foreach (var symbol in Symbols)
            {
                FirstSets.Add(symbol, new HashSet<Symbol>());
                if (symbol.IsTerminal() || symbol.IsEpsilon())
                {
                    FirstSets[symbol].Add(symbol);
                }
            }

            // Copy terminals into first(non terminals)
            foreach (var productionRule in ProductionRules)
            {
                var firstSymbol = productionRule.Body.First();
                if (firstSymbol.IsTerminal())
                {
                    FirstSets[productionRule.Header].Add(firstSymbol);
                }
            }

            var changeHasOccurred = true;
            while (changeHasOccurred)
            {
                changeHasOccurred = false;

                foreach (var productionRule in ProductionRules)
                {
                    var symbolsToAdd = new HashSet<Symbol>();

                    var symbolsThatContainEps = 0;

                    foreach (var symbolInBody in productionRule.Body)
                    {
                        symbolsToAdd.UnionWith(FirstSets[symbolInBody]);

                        if (FirstSets[symbolInBody].Any(e => e.IsEpsilon()))
                            symbolsThatContainEps++;
                        else
                            break;
                    }


                    if (symbolsThatContainEps != productionRule.Body.Count)
                        symbolsToAdd.RemoveWhere(e => e.IsEpsilon());

                    if (!symbolsToAdd.IsSubsetOf(FirstSets[productionRule.Header]))
                    {
                        FirstSets[productionRule.Header].UnionWith(symbolsToAdd);
                        changeHasOccurred = true;
                    }
                }
            }
        }

        public HashSet<Symbol> GetFirstSet(int productionId)
        {

            var productionRule = ProductionRules[productionId];
            var symbolsToAdd = new HashSet<Symbol>();
            var symbolsThatContainEps = 0;

            foreach (var symbolInBody in productionRule.Body)
            {
                symbolsToAdd.UnionWith(FirstSets[symbolInBody]);
                if (FirstSets[symbolInBody].Any(e => e.IsEpsilon()))
                    symbolsThatContainEps++;
                else
                    break;
            }


            if (symbolsThatContainEps != productionRule.Body.Count)
                symbolsToAdd.RemoveWhere(e => e.IsEpsilon());

            if (!symbolsToAdd.IsSubsetOf(FirstSets[productionRule.Header]))
                FirstSets[productionRule.Header].UnionWith(symbolsToAdd);

            return symbolsToAdd;
        }

        public override string ToString()
        {
            var productionRulesString = "\n";
            foreach (var productionRule in ProductionRules)
            {
                productionRulesString += productionRule.ToString();
                productionRulesString += "\n";
            }
            return string.Format("Grammar: Production rules: " + productionRulesString);
        }



        }

    }

