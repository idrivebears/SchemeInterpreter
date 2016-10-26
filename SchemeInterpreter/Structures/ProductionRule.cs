using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Structures
{
    public class ProductionRule
    {
        public ProductionRule(Symbol header, List<Symbol> body, int Caret = 0)
        {
            Header = header;
            Body = body;
        }

        public Symbol Header { get; set; }
        public List<Symbol> Body { get; set; }
        public int Caret { get; set; }

        public override bool Equals(object obj)
        {
            var otherRule = obj as ProductionRule;
            if (otherRule.Header == this.Header && otherRule.Body == this.Body)
                return true;
            return false;
        }

        public override string ToString()
        {
            var bodyString = "\n";
            foreach (var symbol in Body)
            {
                bodyString += symbol.ToString();
                bodyString += "\n";
            }
            return string.Format("Production rule: Header: {0}, Body: {1}, Caret at: {2}", Header, bodyString, Caret);
        }
    }
}
