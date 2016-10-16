using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreterTest.SyntacticAnalysis
{
    [TestClass]
    public class GrammarExample1
    {

        /* Grammar G */

        /* Production rules:
         * E    ->  T E'
         * E'   ->  + T E'
         * E'   ->  eps
         * T    ->  F T'
         * T'   ->  * F T'
         * T'   ->  eps
         * F    ->  ( E )
         * F    ->  id
         */

        /* No terminal symbols
         * { E, E', T, T', F }
         * */
        /* Terminal symbols
         * {id, +, *, (, ), $}
         */

        private Grammar g;

        [TestInitialize]
        public void InitializeGrammar()
        {
            var symbols = new Dictionary<string, Symbol>
            {
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
               // {"$", new Symbol(Symbol.SymTypes.EOS, "$")},
                {"EPSILON", new Symbol(Symbol.SymTypes.Epsilon, "EPSILON")}
            };

            var productionRules = new List<ProductionRule>();

            var productionRuleBody = new List<Symbol> { symbols["T"], symbols["E'"] };
            var productionRule = new ProductionRule(symbols["E"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["+"], symbols["T"], symbols["E'"] };
            productionRule = new ProductionRule(symbols["E'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["EPSILON"] };
            productionRule = new ProductionRule(symbols["E'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["F"], symbols["T'"] };
            productionRule = new ProductionRule(symbols["T"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["*"], symbols["F"], symbols["T'"] };
            productionRule = new ProductionRule(symbols["T'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["EPSILON"] };
            productionRule = new ProductionRule(symbols["T'"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["("], symbols["E"], symbols[")"] };
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["id"] };
            productionRule = new ProductionRule(symbols["F"], productionRuleBody);
            productionRules.Add(productionRule);

            g = new Grammar(productionRules, symbols.Values.ToList());
        }

        [TestMethod]
        public void TestExample1()
        {
            var ll1 = new LL1(g);

            var s = "id+id*id";
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

        [TestMethod]
        public void TestExample2()
        {
            var ll1 = new LL1(g);

            var s = "id+id+id*";
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
            Assert.IsFalse(acc);
        }

        [TestMethod]
        public void TestExample3()
        {
            var ll1 = new LL1(g);

            var s = "a+(b*var1)"; 
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

                [TestMethod]
        public void TestExample4()
        {
            var ll1 = new LL1(g);

            var s = "id*( a+b))";
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
            Assert.Inconclusive();
        }

    }
}
