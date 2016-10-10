using System;
using System.IO;
using System.Text.RegularExpressions;

/*
    Syntax
    TokenName:"REGEX RULE";
     */

namespace SchemeInterpreter.LexerEngine
{
    public static class LexerGenerator
    {

        private static Tuple<string, string> GetMember(string line)
        {
            var memberName = new Regex(@"\([^:)]+\)"); //Get Token Name
            var memberRegEx = new Regex(@":(.*);"); //Get regEx for Token

            var memName = memberName.Match(line);
            var memRegEx = memberRegEx.Match(line);

            return new Tuple<string, string>(memName.Value, memRegEx.Value.Substring(2, memRegEx.Value.Length-4));
            //Trim is to remove :" leading caracters, and to ignore "; trailing caracters
        }

        public static LexerEngine.Lexer Generate(string filename)
        {
            if (!filename.Contains(".miniflex"))
                throw new FileLoadException("File not supported");

            try
            {
                var lexContent = File.ReadAllLines(filename);
                var lexer = new LexerEngine.Lexer();
                foreach (var line in lexContent)
                {
                    var newMember = GetMember(line);
                    Console.WriteLine("Added Toke: {0} Rule {1}", newMember.Item1,@newMember.Item2);

                    lexer.AddDefinition(new TokenDefinition(newMember.Item1, new Regex(@newMember.Item2)));
                }
                return lexer;
            }
            catch
            {
                return null;
            }
        }
    }
}
