using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Structures
{
    public class ProductionRule
    {
        public ProductionRule(Symbol header, List<Symbol> body)
        {
            Header = header;
            Body = body;
        }

        public Symbol Header { get; set; }
        public List<Symbol> Body { get; set; }

        public override string ToString()
        {
            var bodyString = "\n";
            foreach (var symbol in Body)
            {
                bodyString += symbol.ToString();
                bodyString += "\n";
            }
            return string.Format("Production rule: Header: {0}, Body: {1}", Header, bodyString);
        }
    }
}
