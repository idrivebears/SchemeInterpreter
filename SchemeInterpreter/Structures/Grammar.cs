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

            var initialRule = ProductionRules.First();
            FollowSets[initialRule.Header].Add(new Symbol(Symbol.SymTypes.EOS, "$"));

            // Check all productions
            foreach(var header in ProductionRules)
            {
                var currentHeader = header.Header;
                // Look for header in ProductionRule

                foreach (var rule in ProductionRules)
                {
                    if (rule.Body.Contains(currentHeader))
                    {
                        //Check next element in line
                        var occurance = rule.Body.IndexOf(currentHeader);
                        if (occurance < rule.Body.Count)
                        {
                            var next = rule.Body.ElementAt(occurance + 1);

                            // Check if first is an epsilon
                            if (FirstSets[next].Contains(new Symbol(Symbol.SymTypes.Epsilon, "EPSILON")))
                            {
                                //Contains epsilon, we should add the follow of the Header
                                FollowSets[currentHeader].UnionWith(FollowSets[rule.Header]);
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

