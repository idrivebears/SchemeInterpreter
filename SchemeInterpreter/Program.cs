using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchemeInterpreter.Engine;
using SchemeInterpreter.LexerEngine;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;
using SchemeInterpreter.TacoScheme;

namespace SchemeInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheme = new TacoSchemeGrammar();
            var schemeSymbols = new List<Symbol>(scheme.Symbols.Values);
            var schemeGrammar = new Grammar(scheme.productionRules, schemeSymbols);
            var parser = new LR1Table(schemeGrammar, "Scheme.miniflex");
            var source = File.ReadAllText("source.ss");
            var program = parser.Accept(source);

            var input = "";
            do
            {
                Console.Write("Taco> ");
                input = Console.ReadLine();
                if(input == "exit")
                    break;
                try
                {
                    program = parser.Accept(input);
                    //Console.WriteLine(program.Result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("TacoError:: "+ e.Message);
                }
            } while (true);

            Console.WriteLine("TERMINATED");
            Console.Read();
        }
    }
}
