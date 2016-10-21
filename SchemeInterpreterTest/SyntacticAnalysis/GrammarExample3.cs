using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreterTest.SyntacticAnalysis
{
    [TestClass]
    public class GrammarExample3
    {

        /* Grammar G */

        /* Production rules:
        E→TX
        T→( E )| int Y
        X→+E|  ε
        Y→*T| ε

         */

        /* No terminal symbols
         * { E, T, X, Y }
         * */
        /* Terminal symbols
         * {int, +, *, (, ), $}
         */

        private Grammar g;

        [TestInitialize]
        public void InitializeGrammar()
        {
            var symbols = new Dictionary<string, Symbol>
            {
                {"S", new Symbol(Symbol.SymTypes.NoTerminal, "S")},
                {"A", new Symbol(Symbol.SymTypes.NoTerminal, "A")},
                {"B", new Symbol(Symbol.SymTypes.NoTerminal, "B")},
                
                {"a", new Symbol(Symbol.SymTypes.Terminal, "a")},
                {"b", new Symbol(Symbol.SymTypes.Terminal, "b")},
                {"c", new Symbol(Symbol.SymTypes.Terminal, "c")},
                
               // {"$", new Symbol(Symbol.SymTypes.EOS, "$")},
                {"EPSILON", new Symbol(Symbol.SymTypes.Epsilon, "EPSILON")}
            };

            var productionRules = new List<ProductionRule>();

            var productionRuleBody = new List<Symbol> { symbols["a"], symbols["S"], symbols["b"],symbols["A"] };
            var productionRule = new ProductionRule(symbols["S"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["EPSILON"]};
            productionRule = new ProductionRule(symbols["S"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["b"], symbols["a"], symbols["S"], symbols["B"] };
            productionRule = new ProductionRule(symbols["A"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["c"], symbols["S"] };
            productionRule = new ProductionRule(symbols["B"], productionRuleBody);
            productionRules.Add(productionRule);


            g = new Grammar(productionRules, symbols.Values.ToList());
        }

        [TestMethod]
        public void TestExample1()
        {
            var ll1 = new LL1(g);

            var s = "a b b a c";
            Console.WriteLine("String to evaluate: " + s);
            var acc = ll1.Accept(s);
            if (acc)
            {
                Console.WriteLine("String " + s + " is accepted");
            }
            else
            {
                Console.WriteLine("String " + s + " is not accepted");
            }
            Assert.IsTrue(acc);
        }

    }
}
