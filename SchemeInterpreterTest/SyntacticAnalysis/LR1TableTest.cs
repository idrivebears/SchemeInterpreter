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
            var symbols = new Dictionary<string, Symbol>
            {
                {"E'", new Symbol(Symbol.SymTypes.NoTerminal, "E'")},
                {"E", new Symbol(Symbol.SymTypes.NoTerminal, "E")},
                {"T", new Symbol(Symbol.SymTypes.NoTerminal, "T")},
                {"F", new Symbol(Symbol.SymTypes.NoTerminal, "F")},
                {"id", new Symbol(Symbol.SymTypes.Terminal, "id")},
                {"+", new Symbol(Symbol.SymTypes.Terminal, "+")},
                {"*", new Symbol(Symbol.SymTypes.Terminal, "*")},
                {"(", new Symbol(Symbol.SymTypes.Terminal, "(")},
                {")", new Symbol(Symbol.SymTypes.Terminal, ")")}
            };

            var productionRules = new List<ProductionRule>();

            var productionRuleBody = new List<Symbol> { symbols["E"] };
            var productionRule = new ProductionRule(symbols["E'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["E"], symbols["+"], symbols["T"] };
            productionRule = new ProductionRule(symbols["E"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["T"] };
            productionRule = new ProductionRule(symbols["E"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["T"], symbols["*"], symbols["F"] };
            productionRule = new ProductionRule(symbols["T"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["F"] };
            productionRule = new ProductionRule(symbols["T"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["("], symbols["E"], symbols[")"] };
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["id"] };
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            var g = new Grammar(productionRules, symbols.Values.ToList());

            //build table
            var analyzer = new LR1Table(g, "Scheme.miniflex");

            //Assert.IsTrue(analyzer.Accept("id*****id*id"));
        }
    }
}
