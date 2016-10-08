using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchemeInterpreter.LexerEngine;

namespace SchemeInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var lexer = LexerGenerator.Generate("test.miniflex");

            if (lexer != null)
            {
                var tokens = lexer.Tokenize("1 * 2 / 3 + 4 - 5");

                foreach (var token in tokens)
                    Console.WriteLine(token);
            }

            Console.WriteLine("Press ENTER to quit.");
            Console.ReadLine();
            //hi


        }
    }
}
