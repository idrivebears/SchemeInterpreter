using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Structures
{
    public class Symbol
    {
        public enum SymTypes { Terminal, NoTerminal, Epsilon, EOS }

        public SymTypes Type { get; set; }
        public string Value { get; set; }

        public Symbol(SymTypes type, string value)
        {
            Type = type;
            Value = value;
        }

        public bool IsTerminal()
        {
            return Type.Equals(SymTypes.Terminal);
        }

        public bool IsNonTerminal()
        {
            return Type.Equals(SymTypes.NoTerminal);
        }

        public bool IsEpsilon()
        {
            return Type.Equals(SymTypes.Epsilon);
        }

        public override string ToString()
        {
            return string.Format("Symbol: Type: {0}, Value: {1}", Type, Value);
        }
    }
}
