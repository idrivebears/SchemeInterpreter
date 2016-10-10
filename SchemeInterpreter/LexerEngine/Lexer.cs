using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SchemeInterpreter.Lexer;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter.LexerEngine
{
    //Implementation by Drew Miller
    public class Lexer : ILexer
    {
        readonly Regex _endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
        readonly IList<TokenDefinition> _tokenDefinitions = new List<TokenDefinition>();

        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            _tokenDefinitions.Add(tokenDefinition);
        }

        public IEnumerable<Token> Tokenize(string source)
        {
            var currentIndex = 0;
            var currentLine = 1;
            var currentColumn = 0;

            while (currentIndex < source.Length)
            {
                TokenDefinition matchedDefinition = null;
                var matchLength = 0;

                foreach (var rule in _tokenDefinitions)
                {
                    var match = rule.Regex.Match(source, currentIndex);

                    if (match.Success && (match.Index - currentIndex) == 0)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        break;
                    }
                }

                if (matchedDefinition == null)
                {
                    throw new Exception(string.Format("Unrecognized symbol '{0}' at index {1} (line {2}, column {3}).", source[currentIndex], currentIndex, currentLine, currentColumn));
                }
                else
                {
                    var value = source.Substring(currentIndex, matchLength);

                    if (!matchedDefinition.IsIgnored)
                        yield return new Token(matchedDefinition.Type, value, new TokenPosition(currentIndex, currentLine, currentColumn));

                    var endOfLineMatch = _endOfLineRegex.Match(value);
                    if (endOfLineMatch.Success)
                    {
                        currentLine += 1;
                        currentColumn = value.Length - (endOfLineMatch.Index + endOfLineMatch.Length);
                    }
                    else
                    {
                        currentColumn += matchLength;
                    }

                    currentIndex += matchLength;
                }
            }

            yield return new Token("(end)", null, new TokenPosition(currentIndex, currentLine, currentColumn));
        }
    }
}
