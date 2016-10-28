using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreterTest.SyntacticAnalysis
{
    [TestClass]
    public class LR1TableTest
    {
        [TestMethod]
        public void TestTableAcceptor()
        {
            Grammar g;
            Dictionary<string, Symbol> symbols;

            symbols = new Dictionary<string, Symbol>
            {
                {"S", new Symbol(Symbol.SymTypes.NoTerminal, "S")},
                {"P", new Symbol(Symbol.SymTypes.NoTerminal, "P")},
                {"M", new Symbol(Symbol.SymTypes.NoTerminal, "M")},
                {"E", new Symbol(Symbol.SymTypes.NoTerminal, "E")},
                {"K", new Symbol(Symbol.SymTypes.NoTerminal, "K")},
                {"q", new Symbol(Symbol.SymTypes.Terminal, "q")}, 
                {"n", new Symbol(Symbol.SymTypes.Terminal, "n")},
                {"t", new Symbol(Symbol.SymTypes.Terminal, "t")},
                {"r", new Symbol(Symbol.SymTypes.Terminal, "r")},
                {"u", new Symbol(Symbol.SymTypes.Terminal, "u")}
            };

            var productionRules = new List<ProductionRule>();

            var productionRuleBody = new List<Symbol> { symbols["P"], symbols["M"] };
            var productionRule = new ProductionRule(symbols["S"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["E"] };
            productionRule = new ProductionRule(symbols["P"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["q"] };
            productionRule = new ProductionRule(symbols["P"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["n"], symbols["E"] };
            productionRule = new ProductionRule(symbols["M"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["t"] };
            productionRule = new ProductionRule(symbols["M"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["r"], symbols["K"] };
            productionRule = new ProductionRule(symbols["E"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["M"] };
            productionRule = new ProductionRule(symbols["E"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["u"], symbols["K"] };
            productionRule = new ProductionRule(symbols["K"], productionRuleBody);
            productionRules.Add(productionRule);

            g = new Grammar(productionRules, symbols.Values.ToList());

            //build table
            var analyzer = new LR1Table(g);

            Assert.IsTrue(analyzer.Accept("qnt"));
        }
    }
}
