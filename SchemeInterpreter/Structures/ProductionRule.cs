﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Structures
{
    public class ProductionRule
    {
        

        public Symbol Header { get; set; }
        public List<Symbol> Body { get; set; }
        public int Caret { get; set; }

        public ProductionRule(Symbol header, List<Symbol> body, int Caret = 0)
        {
            Header = header;
            Body = body;
        }

        public ProductionRule(ProductionRule productionRule)
        {
            Header = productionRule.Header;
            Body = productionRule.Body;
            Caret = productionRule.Caret;
        }

        public override bool Equals(object obj)
        {
            var otherRule = obj as ProductionRule;
            return (otherRule.Header == Header && otherRule.Body == Body && otherRule.Caret == Caret);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
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
