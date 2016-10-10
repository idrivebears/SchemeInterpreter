using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Structures
{
    public class Symbol
    {
        public static string Terminal = "TERMINAL";
        public static string NoTerminal = "NO_TERMINAL";
        public static string Epsilon = "EPSILON";

        public Symbol(String type, String value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; set; }
        public string Value { get; set; }

        public List<Symbol> FirstSymbols;
        public List<Symbol> FollowSymbols;

        public bool IsTerminal()
        {
            return Type.Equals(Symbol.Terminal);
        }

        public bool IsNonTerminal()
        {
            return Type.Equals(Symbol.NoTerminal);
        }

        public bool IsEpsilon()
        {
            return Type.Equals(Symbol.Epsilon);
        }

        public override string ToString()
        {
            return string.Format("Symbol: Type: {0}, Value: {1}", Type, Value);
        }
    }
}
