﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreterTest.SyntacticAnalysis
{
    [TestClass]
    public class LR1TableTest2
    {
        [TestMethod]
        public void TestTableAcceptor()
        {
            var symbols = new Dictionary<string, Symbol>
            {
                {"S", new Symbol(Symbol.SymTypes.NoTerminal, "S")},
                {"E", new Symbol(Symbol.SymTypes.NoTerminal, "E")},
                {"E'", new Symbol(Symbol.SymTypes.NoTerminal, "E'")},
                {"T", new Symbol(Symbol.SymTypes.NoTerminal, "T")},
                {"T'", new Symbol(Symbol.SymTypes.NoTerminal, "T'")},
                {"F", new Symbol(Symbol.SymTypes.NoTerminal, "F")},
                {"id", new Symbol(Symbol.SymTypes.Terminal, "id")},
                {"+", new Symbol(Symbol.SymTypes.Terminal, "+")},
                {"*", new Symbol(Symbol.SymTypes.Terminal, "*")},
                {"(", new Symbol(Symbol.SymTypes.Terminal, "(")},
                {")", new Symbol(Symbol.SymTypes.Terminal, ")")},
                {"EPSILON", new Symbol(Symbol.SymTypes.Epsilon, "EPSILON")}
            };

            var productionRules = new List<ProductionRule>();


            var productionRuleBody = new List<Symbol> { symbols["Program"] };
            var productionRule = new ProductionRule(symbols["Start"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["Form_list"] };
            productionRule = new ProductionRule(symbols["Program"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["Form_list"], symbols["Form"]};
            productionRule = new ProductionRule(symbols["Form_list"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["Definition"] };
            productionRule = new ProductionRule(symbols["Form"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["Expression"] };
            productionRule = new ProductionRule(symbols["Form"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["Definition_list"], symbols["Definition"] };
            productionRule = new ProductionRule(symbols["Definition_list"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["Variable_definition"] };
            productionRule = new ProductionRule(symbols["Definition"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["(Paren"] };
            productionRule = new ProductionRule(symbols["Definition"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["Variable_definition"] };
            productionRule = new ProductionRule(symbols["Definition"], productionRuleBody);
            productionRules.Add(productionRule);

            var g = new Grammar(productionRules, symbols.Values.ToList());

            //build table
            var analyzer = new LR1Table(g);
            analyzer.Print();

            Assert.IsTrue(analyzer.Accept("(id)"));
        }
    }
}
