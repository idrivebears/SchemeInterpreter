using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter.Structures;

namespace SchemeInterpreterTest.Structures
{
    /// <summary>
    /// Summary description for SymbolTest
    /// </summary>
    [TestClass]
    public class SymbolTest
    {
        public SymbolTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
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
        public void TestEpsilonSet()
        {
            HashSet<Symbol> set = new HashSet<Symbol>();

            set.Add(new Symbol(Symbol.Epsilon, "EPSILON"));

            Assert.IsFalse(set.Contains(new Symbol(Symbol.Epsilon, "EPSILON")));

        }
    }
}
