using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreterTest.SyntacticAnalysis
{
    [TestClass]
    public class GrammarExample2
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
                {"E", new Symbol(Symbol.SymTypes.NoTerminal, "E")},
                {"T", new Symbol(Symbol.SymTypes.NoTerminal, "T")},
                {"X", new Symbol(Symbol.SymTypes.NoTerminal, "X")},
                {"Y", new Symbol(Symbol.SymTypes.NoTerminal, "Y")},
                {"id", new Symbol(Symbol.SymTypes.Terminal, "id")},
                {"+", new Symbol(Symbol.SymTypes.Terminal, "+")},
                {"*", new Symbol(Symbol.SymTypes.Terminal, "*")},
                {"(", new Symbol(Symbol.SymTypes.Terminal, "(")},
                {")", new Symbol(Symbol.SymTypes.Terminal, ")")},
               // {"$", new Symbol(Symbol.SymTypes.EOS, "$")},
                {"EPSILON", new Symbol(Symbol.SymTypes.Epsilon, "EPSILON")}
            };

            var productionRules = new List<ProductionRule>();

            var productionRuleBody = new List<Symbol> { symbols["T"], symbols["X"] };
            var productionRule = new ProductionRule(symbols["E"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["("], symbols["E"], symbols[")"] };
            productionRule = new ProductionRule(symbols["T"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["id"], symbols["Y"] };
            productionRule = new ProductionRule(symbols["T"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["+"], symbols["E"] };
            productionRule = new ProductionRule(symbols["X"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["EPSILON"] };
            productionRule = new ProductionRule(symbols["X"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["EPSILON"] };
            productionRule = new ProductionRule(symbols["Y"], productionRuleBody);
            productionRules.Add(productionRule);

            productionRuleBody = new List<Symbol> { symbols["*"], symbols["T"] };
            productionRule = new ProductionRule(symbols["Y"], productionRuleBody);
            productionRules.Add(productionRule);


            g = new Grammar(productionRules, symbols.Values.ToList());
        }

        [TestMethod]
        public void TestExample1()
        {
            var ll1 = new LL1(g);

            var s = "int +(+ int";
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
        public void TestExample2()
        {
            var ll1 = new LL1(g);

            var s = "int+int+int*";
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

            var s = "int +( int * int)"; 
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

            var s = "int *( int + int))";
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

    }
}
