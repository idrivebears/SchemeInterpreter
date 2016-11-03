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
                {"S'", new Symbol(Symbol.SymTypes.NoTerminal, "S'")},
                {"S", new Symbol(Symbol.SymTypes.NoTerminal, "S")},
                {"F", new Symbol(Symbol.SymTypes.NoTerminal, "F")},
                {"(", new Symbol(Symbol.SymTypes.Terminal, "(")},
                {")", new Symbol(Symbol.SymTypes.Terminal, ")")},
                {"*", new Symbol(Symbol.SymTypes.Terminal, "*")},
                {"id", new Symbol(Symbol.SymTypes.Terminal, "id")},
            };

            var productionRules = new List<ProductionRule>();

            var productionRuleBody = new List<Symbol> { symbols["S"] };
            var productionRule = new ProductionRule(symbols["S'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["S"], symbols["*"], symbols["F"] };
            productionRule = new ProductionRule(symbols["S"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["F"] };
            productionRule = new ProductionRule(symbols["S"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["("], symbols["S"], symbols[")"] };
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["id"] };
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            var g = new Grammar(productionRules, symbols.Values.ToList());

            //build table
            var analyzer = new LR1Table(g);

            //Assert.IsTrue(analyzer.Accept("id*id"));
            Assert.IsTrue(analyzer.Accept("id*(id*id)"));
            Assert.IsFalse(analyzer.Accept("id*(id*id))"));
            Assert.IsTrue(analyzer.Accept("((id*id)*(id*id))"));
            // Assert.IsTrue(analyzer.Accept("(id*id)*id"));
            //Assert.IsFalse(analyzer.Accept("(*id))"));
        }
    }
}
