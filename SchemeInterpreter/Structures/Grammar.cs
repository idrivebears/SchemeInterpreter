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

        public Grammar(IEnumerable<ProductionRule> productionRules, IEnumerable<Symbol> symbols)
        {
            ProductionRules = new List<ProductionRule>(productionRules);
            Symbols = new List<Symbol>(symbols);
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

