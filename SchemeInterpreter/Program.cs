using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchemeInterpreter.LexerEngine;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var lexer = LexerGenerator.Generate("camTest.miniflex");
            var source = File.ReadAllText("source.ss");
            var tokens = lexer.Tokenize(source);
            foreach (var token in tokens)
                Console.WriteLine("Type::'{0}' Value::{1}", token.Type, token.Value);

            Console.WriteLine("Press ENTER to quit.");
            Console.ReadLine();

        }
    }
}
