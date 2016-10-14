using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;

namespace SchemeInterpreterTest.Structures
{
    /// <summary>
    ///             
    /// /* First and follow of grammar G*/

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

    /// </summary>
    [TestClass]
    public class GrammarTest
    {
        private Grammar grammar;
        private Dictionary<string, Symbol> symbols;

        public GrammarTest()
        {
            symbols = new Dictionary<string, Symbol>
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
                {"$", new Symbol(Symbol.SymTypes.EOS, "$")},
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

            grammar = new Grammar(productionRules, symbols.Values.ToList());
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGenerateFirstSets()
        {
            grammar.GenerateFirstSets();

            var first = grammar.GetFirstSet(2);

            var firstofE = new HashSet<Symbol>
            {
                symbols["("],
                symbols["id"]
            };

            Assert.IsTrue(grammar.FirstSets[symbols["E"]].SetEquals(firstofE));

            var firstofEp = new HashSet<Symbol>
            {
                symbols["+"],
                symbols["EPSILON"]
            };

            Assert.IsTrue(grammar.FirstSets[symbols["E'"]].SetEquals(firstofEp));
        }

        [TestMethod]
        public void TestGenerateFollowSets()
        {
            grammar.GenerateFirstSets();
            grammar.GenerateFollowSets();

            var followOfE = new HashSet<Symbol>
            {
                symbols[")"],
                symbols["$"]
            };
            Assert.IsTrue(grammar.FirstSets[symbols["E"]].SetEquals(followOfE));
        }

    }
}
