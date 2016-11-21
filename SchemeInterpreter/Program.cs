using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchemeInterpreter.LexerEngine;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = LexerGenerator.Generate("Scheme.miniflex");
            var input = File.ReadAllText("source.ss");
            var tokens = lexer.Tokenize(input);

            foreach (var token in tokens)
                Console.WriteLine(token);
            Console.WriteLine("It works!"); //this is a change
        }
    }
}
