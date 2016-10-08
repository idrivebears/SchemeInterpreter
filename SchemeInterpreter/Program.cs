using System;
using System.Collections.Generic;
using System.IO;
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
            
            //var lexer = LexerGenerator.Generate("camTest.miniflex");

            //Scheme hardcoded tokenizer
            var lexer = new Lexer();
            lexer.AddDefinition(new TokenDefinition("(InlineComment)", new Regex(@";;[^\n]*\n")));
            lexer.AddDefinition(new TokenDefinition("(Keyword)", new Regex(@"access|define-syntax|macro|and|delay|make-environment|begin|do|named-lambda|case|fluid-let|or|cond|if|quasiquote|cons-stream|in-package|quote|declare|lambda|scode-quote|default-object?|sequence|set!|define-integrable|let-syntax|the-environment|define-macro|letrec|unassigned?|define-structure|local-declare|using-syntax|let|define")));
            lexer.AddDefinition(new TokenDefinition("(Whitespace)", new Regex(@"\s+"), true));
            lexer.AddDefinition(new TokenDefinition("(OpPlus)", new Regex(@"\+")));
            lexer.AddDefinition(new TokenDefinition("(OpMinus)", new Regex(@"-")));
            lexer.AddDefinition(new TokenDefinition("(OpMult)", new Regex(@"\*")));
            lexer.AddDefinition(new TokenDefinition("(OpDiv)", new Regex(@"/")));
            lexer.AddDefinition(new TokenDefinition("(OpMod)", new Regex(@"modulo")));
            lexer.AddDefinition(new TokenDefinition("(OpQuotient)", new Regex(@"quotient")));
            lexer.AddDefinition(new TokenDefinition("(OpEqual)", new Regex(@"equal\?")));
            lexer.AddDefinition(new TokenDefinition("(OpEqv)", new Regex(@"eqv\?")));
            lexer.AddDefinition(new TokenDefinition("(OpEq)", new Regex(@"eq\?")));
            lexer.AddDefinition(new TokenDefinition("(OpGreaterEq)", new Regex(@">=")));
            lexer.AddDefinition(new TokenDefinition("(OpLessEq)", new Regex(@"<=")));
            lexer.AddDefinition(new TokenDefinition("(OpEqualN)", new Regex(@"=")));
            lexer.AddDefinition(new TokenDefinition("(OpLess)", new Regex(@"<")));
            lexer.AddDefinition(new TokenDefinition("(OpGreater)", new Regex(@">")));
            lexer.AddDefinition(new TokenDefinition("(Number)", new Regex(@"(\+|-)?[0-9]+")));
            lexer.AddDefinition(new TokenDefinition("(Identifiers)", new Regex(@"([A-z!$%&*:/<=>?~_^])(([A-z0-9!$%&*:/<=>?~_^.+-])*[+-]*( \.\.\.)*)")));
            lexer.AddDefinition(new TokenDefinition("(String)", new Regex(@"("")([^""]*)("")")));
            lexer.AddDefinition(new TokenDefinition("(Delimiter)", new Regex(@"[();""'`|{}[]|]")));

            if (lexer != null)
            {
                var source = File.ReadAllText("source.ss");
                var tokens = lexer.Tokenize(source);

                foreach (var token in tokens)
                    Console.WriteLine(string.Format("Type::'{0}' Value::{1}", token.Type, token.Value));
            }

            Console.WriteLine("Press ENTER to quit.");
            Console.ReadLine();
        }
    }
}
