using System;

namespace SimpleLexer
{
    public class TokenPosition
    {
        public TokenPosition(int index, int line, int column)
        {
            //Implementation by Drew Miller
            Index = index;
            Line = line;
            Column = column;
        }

        public int Column { get; private set; }
        public int Index { get; private set; }
        public int Line { get; private set; }
    }
}
