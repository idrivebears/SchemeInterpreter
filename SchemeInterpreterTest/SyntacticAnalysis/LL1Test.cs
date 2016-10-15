using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreterTest.SyntacticAnalysis
{
    [TestClass]
    public class LL1Test
    {
        [TestMethod]
        public void TestLlEpsilonAcceptor()
        {
            Grammar g;
            Dictionary<string, Symbol> symbols;

            symbols = new Dictionary<string, Symbol>
            {
                {"E", new Symbol(Symbol.SymTypes.NoTerminal, "E")},
                {"E'", new Symbol(Symbol.SymTypes.NoTerminal, "E'")},
                {"T", new Symbol(Symbol.SymTypes.NoTerminal, "T")},
                {"T'", new Symbol(Symbol.SymTypes.NoTerminal, "T'")},
                {"F", new Symbol(Symbol.SymTypes.NoTerminal, "F")},
                {"id", new Symbol(Symbol.SymTypes.Terminal, "id")}, //ID
                {"+", new Symbol(Symbol.SymTypes.Terminal, "+")},
                {"*", new Symbol(Symbol.SymTypes.Terminal, "*")},
                {"(", new Symbol(Symbol.SymTypes.Terminal, "(")},
                {")", new Symbol(Symbol.SymTypes.Terminal, ")")},
                {"EPSILON", new Symbol(Symbol.SymTypes.Epsilon, "EPSILON")}
            };

            var productionRules = new List<ProductionRule>();

            var productionRuleBody = new List<Symbol> { symbols["T"], symbols["E'"] };
            var productionRule = new ProductionRule(symbols["E"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["+"], symbols["T"], symbols["E'"] };
            productionRule = new ProductionRule(symbols["E'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["EPSILON"]};
            productionRule = new ProductionRule(symbols["E'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["F"], symbols["T'"]};
            productionRule = new ProductionRule(symbols["T"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["*"], symbols["F"], symbols["T'"] };
            productionRule = new ProductionRule(symbols["T'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["EPSILON"] };
            productionRule = new ProductionRule(symbols["T'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["("], symbols["id"], symbols[")"] };
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["id"]};
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            g = new Grammar(productionRules, symbols.Values.ToList());

            var ll1 = new LL1(g);
            Assert.IsTrue(ll1.Accept("id+id*id"));
            Assert.IsTrue(ll1.Accept("id + id + id * id + id * id"));
        }
    }
}
