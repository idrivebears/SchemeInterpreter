using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
    Syntax
    TokenName:"REGEX RULE";
     */

namespace SchemeInterpreter.LexerEngine
{
    public static class LexerGenerator
    {

        private static Tuple<string, string> ExtractMembers(String statement)
        {
            var tokenName = "";
            var rule = "";

            int i = 0;
            for (; i < statement.Length; i++)
            {
                if (statement[i] == ':')
                    break;
                else
                    tokenName += statement[i];
            }

            i++;
            bool ruleStarted = false;
            bool ruleFinished = false;
            if (statement[i] == '"')
            {
                ruleStarted = true;
            }
            i++;

            for(; i < statement.Length && ruleStarted && !ruleFinished; i++)
            {
                if (statement[i] != '"')
                {
                    rule += statement[i];
                }
                else
                {
                    if (statement[i + 1] == ';')
                        ruleFinished = true;
                    else
                        rule += statement[i];
                }
            }

            if (!ruleFinished)
            {
                throw new Exception("Syntax error in .miniflex file.");    
            }

            return Tuple.Create(tokenName, rule);
        }

        public static Lexer Generate(String filename)
        {
            if (!filename.Contains(".miniflex"))
                throw new FileLoadException("File not supported");
            try
            {
                var lexContent = File.ReadAllLines(filename);
                var lexer = new Lexer();

                foreach (var line in lexContent)
                {
                    var t = LexerGenerator.ExtractMembers(line);
                    lexer.AddDefinition(new TokenDefinition(t.Item1, new System.Text.RegularExpressions.Regex(t.Item2)));
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
