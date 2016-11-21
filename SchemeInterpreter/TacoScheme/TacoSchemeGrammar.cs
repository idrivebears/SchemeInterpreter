using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreter.TacoScheme
{
    class TacoSchemeGrammar
    {
        public Dictionary<string, Symbol> Symbols;
        public TacoSchemeGrammar()
        {
            Symbols = new Dictionary<string, Symbol>
                {
                    {"Start", new Symbol(Symbol.SymTypes.NoTerminal, "Start")},
                    {"Program", new Symbol(Symbol.SymTypes.NoTerminal, "Program")},
                    {"Form_list", new Symbol(Symbol.SymTypes.NoTerminal, "Form_list")},
                    {"Form", new Symbol(Symbol.SymTypes.NoTerminal, "Form")},
                    {"Definition_list", new Symbol(Symbol.SymTypes.NoTerminal, "Definition_list")},
                    {"Definition", new Symbol(Symbol.SymTypes.NoTerminal, "Definition")},
                    {"Variable_definition", new Symbol(Symbol.SymTypes.NoTerminal, "Variable_definition")},
                    {"Variable_list", new Symbol(Symbol.SymTypes.NoTerminal, "Variable_list")},
                    {"Variable", new Symbol(Symbol.SymTypes.NoTerminal, "Variable")},
                    {"Body", new Symbol(Symbol.SymTypes.NoTerminal, "Body")},
                    {"Expression_list", new Symbol(Symbol.SymTypes.NoTerminal, "Expression_list")},
                    {"Expression", new Symbol(Symbol.SymTypes.NoTerminal, "Expression")},
                    {"Constant", new Symbol(Symbol.SymTypes.NoTerminal, "Constant")},
                    {"Formals", new Symbol(Symbol.SymTypes.NoTerminal, "Formals")},
                    {"Variable_list_positive", new Symbol(Symbol.SymTypes.NoTerminal, "Variable_list_positive")},
                    {"Application", new Symbol(Symbol.SymTypes.NoTerminal, "Application")},
                    {"Datum", new Symbol(Symbol.SymTypes.NoTerminal, "Datum")},
                    {"Datum_list_positive", new Symbol(Symbol.SymTypes.NoTerminal, "Datum_list_positive")},
                    {"Datum_list", new Symbol(Symbol.SymTypes.NoTerminal, "Datum_list")},
                    {"Boolean", new Symbol(Symbol.SymTypes.NoTerminal, "Boolean")},
                    {"Symbol", new Symbol(Symbol.SymTypes.NoTerminal, "Symbol")},
                    {"List", new Symbol(Symbol.SymTypes.NoTerminal, "List")},
                    {"Abbreviation", new Symbol(Symbol.SymTypes.NoTerminal, "Abbreviation")},

                    {"id", new Symbol(Symbol.SymTypes.Terminal, "Variable")},
                    {"+", new Symbol(Symbol.SymTypes.Terminal, "+")},
                    {"*", new Symbol(Symbol.SymTypes.Terminal, "*")},
                    {"(", new Symbol(Symbol.SymTypes.Terminal, "(")},
                    {")", new Symbol(Symbol.SymTypes.Terminal, ")")}
                };
        }
    }
}
