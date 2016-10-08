using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchemeInterpreter.Lexer;

namespace SchemeInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer();

            lexer.AddDefinition(new TokenDefinition(
                "(Operator)",
                new Regex(@"\*|\/|\+|\-")));

            lexer.AddDefinition(new TokenDefinition(
                "(literal)",
                new Regex(@"\d+")));


            lexer.AddDefinition(new TokenDefinition(
                "(white-space)",
                new Regex(@"\s+"),
                true));

            var tokens = lexer.Tokenize("1 * 2 / 3 + 4 - 5");

            foreach (var token in tokens)
                Console.WriteLine(token);

            Console.WriteLine("Press ENTER to quit.");
            Console.ReadLine();
        }
    }
}
