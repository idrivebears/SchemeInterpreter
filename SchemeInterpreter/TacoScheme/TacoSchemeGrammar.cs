using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.Engine;
using SchemeInterpreter.Structures;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreter.TacoScheme
{
    class TacoSchemeGrammar
    {
        public Dictionary<string, Symbol> Symbols;
        public List<ProductionRule> productionRules;
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
                    {"Application", new Symbol(Symbol.SymTypes.NoTerminal, "Application")},
                    {"Datum", new Symbol(Symbol.SymTypes.NoTerminal, "Datum")},
                    {"Datum_list", new Symbol(Symbol.SymTypes.NoTerminal, "Datum_list")},
                    {"List", new Symbol(Symbol.SymTypes.NoTerminal, "List")},


                    {"(InlineComment)", new Symbol(Symbol.SymTypes.Terminal, "(InlineComment)")},
                    {"(Keyword)", new Symbol(Symbol.SymTypes.Terminal, "(Keyword)")},
                    {"(Begin)", new Symbol(Symbol.SymTypes.Terminal, "(Begin)")},
                    {"(Define)", new Symbol(Symbol.SymTypes.Terminal, "(Define)")},
                    {"(Point)", new Symbol(Symbol.SymTypes.Terminal, "(Point)")},
                    {"(Lambda)", new Symbol(Symbol.SymTypes.Terminal, "(Lambda)")},
                    {"(If)", new Symbol(Symbol.SymTypes.Terminal, "(If)")},
                    {"(Boolean)", new Symbol(Symbol.SymTypes.Terminal, "(Boolean)")},
                    {"(white-space)", new Symbol(Symbol.SymTypes.Terminal, "(white-space)")},
                    {"(OpPlus)", new Symbol(Symbol.SymTypes.Terminal, "(OpPlus)")},
                    {"(OpMinus)", new Symbol(Symbol.SymTypes.Terminal, "(OpMinus)")},
                    {"(OpMult)", new Symbol(Symbol.SymTypes.Terminal, "(OpMult)")},
                    {"(OpDiv)", new Symbol(Symbol.SymTypes.Terminal, "(OpDiv)")},
                    {"(OpMod)", new Symbol(Symbol.SymTypes.Terminal, "(OpMod)")},
                    {"(OpQuotient)", new Symbol(Symbol.SymTypes.Terminal, "(OpQuotient)")},
                    {"(OpEqual)", new Symbol(Symbol.SymTypes.Terminal, "(OpEqual)")},
                    {"(OpEqv)", new Symbol(Symbol.SymTypes.Terminal, "(OpEqv)")},
                    {"(OpEq)", new Symbol(Symbol.SymTypes.Terminal, "(OpEq)")},
                    {"(OpGreaterEq)", new Symbol(Symbol.SymTypes.Terminal, "(OpGreaterEq)")},
                    {"(OpLessEq)", new Symbol(Symbol.SymTypes.Terminal, "(OpLessEq)")},
                    {"(OpEqualN)", new Symbol(Symbol.SymTypes.Terminal, "(OpEqualN)")},
                    {"(OpLess)", new Symbol(Symbol.SymTypes.Terminal, "(OpLess)")},
                    {"(OpGreater)", new Symbol(Symbol.SymTypes.Terminal, "(OpGreater)")},
                    {"(Number)", new Symbol(Symbol.SymTypes.Terminal, "(Number)")},
                    {"(Identifier)", new Symbol(Symbol.SymTypes.Terminal, "(Identifier)")},
                    {"(String)", new Symbol(Symbol.SymTypes.Terminal, "(String)")},
                    {"(SquareBracketOpen)", new Symbol(Symbol.SymTypes.Terminal, "(SquareBracketOpen)")},
                    {"(SquareBracketClose)", new Symbol(Symbol.SymTypes.Terminal, "(SquareBracketClose)")},
                    {"(ParentOpen)", new Symbol(Symbol.SymTypes.Terminal, "(ParentOpen)")},
                    {"(ParentClose)", new Symbol(Symbol.SymTypes.Terminal, "(ParentClose)")},
                    {"(Apostrophe)", new Symbol(Symbol.SymTypes.Terminal, "(Apostrophe)")},
                    {"(CurlyBracketOpen)", new Symbol(Symbol.SymTypes.Terminal, "(CurlyBracketOpen)")},
                    {"(CurlyBracketClose)", new Symbol(Symbol.SymTypes.Terminal, "(CurlyBracketClose)")},

                    {"Epsilon", new Symbol(Symbol.SymTypes.Epsilon, "Epsilon")}
                };

            productionRules = new List<ProductionRule>();
            var productionRuleBody = new List<Symbol> { Symbols["Program"] };
            var productionRule = new ProductionRule( Symbols["Start"], productionRuleBody, 0, SemanticLibrary.AcRepeater );
            productionRules.Add(productionRule);


            //Program -> Form_list
            productionRuleBody = new List<Symbol> { Symbols["Form_list"] };
            productionRule = new ProductionRule(Symbols["Program"], productionRuleBody, 0, SemanticLibrary.AcRepeater);
            productionRules.Add(productionRule);

            //Form_list -> Form_list Form | Form
            productionRuleBody = new List<Symbol> { Symbols["Form_list"], Symbols["Form"] };
            productionRule = new ProductionRule(Symbols["Form_list"], productionRuleBody);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["Form"]};
            productionRule = new ProductionRule(Symbols["Form_list"], productionRuleBody, 0, SemanticLibrary.AcRepeater);
            productionRules.Add(productionRule);

            //Form -> Definition | Expression
            productionRuleBody = new List<Symbol> { Symbols["Definition"] };
            productionRule = new ProductionRule(Symbols["Form"], productionRuleBody, 0, SemanticLibrary.AcRepeater);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["Expression"] };
            productionRule = new ProductionRule(Symbols["Form"], productionRuleBody, 0, SemanticLibrary.AcRepeater);
            productionRules.Add(productionRule);

            //Definition -> Variable_definition | (begin Definition_list)
            productionRuleBody = new List<Symbol> { Symbols["Variable_definition"] };
            productionRule = new ProductionRule(Symbols["Definition"], productionRuleBody, 0, SemanticLibrary.AcRepeater);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["(Begin)"], Symbols["Definition_list"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Definition"], productionRuleBody);
            productionRules.Add(productionRule);


            /*Variable_definition -> (define Variable Expression) 
             *                      | (define (Variable Variable_list) Body) 
             *                      | (define (Variable Variable_list point Variable) Body)
             */
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["(Define)"], Symbols["Variable"], Symbols["Expression"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Variable_definition"], productionRuleBody, 0, SemanticLibrary.AcDefineVar);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["(Define)"], Symbols["(ParentOpen)"], Symbols["Variable"], Symbols["Variable_list"], Symbols["(ParentClose)"], Symbols["Body"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Variable_definition"], productionRuleBody);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["(Define)"], Symbols["(ParentOpen)"], Symbols["Variable"], 
                Symbols["Variable_list"], Symbols["(Point)"], Symbols["Variable"], Symbols["(ParentClose)"], Symbols["Body"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Variable_definition"], productionRuleBody);
            productionRules.Add(productionRule);


            //Definition_list -> Definition_list Definition | EPS
            productionRuleBody = new List<Symbol> { Symbols["Definition_list"], Symbols["Definition"] };
            productionRule = new ProductionRule(Symbols["Definition_list"], productionRuleBody);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> {Symbols["Epsilon"] };
            productionRule = new ProductionRule(Symbols["Definition_list"], productionRuleBody);
            productionRules.Add(productionRule);

            //Variable_list -> Variable_list Variable | EPS
            productionRuleBody = new List<Symbol> { Symbols["Variable_list"], Symbols["Variable"] };
            productionRule = new ProductionRule(Symbols["Variable_list"], productionRuleBody);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["Epsilon"]};
            productionRule = new ProductionRule(Symbols["Variable_list"], productionRuleBody);
            productionRules.Add(productionRule);


            //Variable -> Identifier
            productionRuleBody = new List<Symbol> { Symbols["(Identifier)"]};
            productionRule = new ProductionRule(Symbols["Variable"], productionRuleBody, 0, SemanticLibrary.AcBuildVariableFromId);
            productionRules.Add(productionRule);

            //Body -> Definition_list Expression_list
            productionRuleBody = new List<Symbol> { Symbols["Definition_list"], Symbols["Expression_list"] };
            productionRule = new ProductionRule(Symbols["Body"], productionRuleBody);
            productionRules.Add(productionRule);

            //Expression_list -> Expression Expression_list | EPS
            productionRuleBody = new List<Symbol> { Symbols["Expression"], Symbols["Expression_list"] };
            productionRule = new ProductionRule(Symbols["Expression_list"], productionRuleBody, 0, SemanticLibrary.AcBuildRightList);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["Epsilon"] };
            productionRule = new ProductionRule(Symbols["Expression_list"], productionRuleBody, 0, SemanticLibrary.AcCreateList);
            productionRules.Add(productionRule);

            /*Expression -> Constant 
            *             | Variable
            *             | 'Datum 
            *             | (lambda Formals Body)
            *             | (if Expression Expression Expression)
            *             | (if Expression Expression)
            *             | Application
            */
            productionRuleBody = new List<Symbol> { Symbols["Constant"]};
            productionRule = new ProductionRule(Symbols["Expression"], productionRuleBody, 0, SemanticLibrary.AcRepeater);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["Variable"] };
            productionRule = new ProductionRule(Symbols["Expression"], productionRuleBody, 0, SemanticLibrary.AcLookupSym);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(Apostrophe)"], Symbols["Datum"] };
            productionRule = new ProductionRule(Symbols["Expression"], productionRuleBody, 0, SemanticLibrary.AcRepeaterIgnoreFirst);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["(Lambda)"], Symbols["Formals"], Symbols["Body"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Expression"], productionRuleBody);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["(If)"], Symbols["Expression"], Symbols["Expression"], Symbols["Expression"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Expression"], productionRuleBody, 0, SemanticLibrary.AcHandleIf);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["Application"]};
            productionRule = new ProductionRule(Symbols["Expression"], productionRuleBody, 0, SemanticLibrary.AcRepeater);
            productionRules.Add(productionRule);

            /* Constant -> boolean 
                         | number
                         | string 
            */
            productionRuleBody = new List<Symbol> { Symbols["(Boolean)"] };
            productionRule = new ProductionRule(Symbols["Constant"], productionRuleBody, 0, SemanticLibrary.AcReduceBoolean );
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(Number)"] };
            productionRule = new ProductionRule(Symbols["Constant"], productionRuleBody, 0, SemanticLibrary.AcReduceNumber);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(String)"] };
            productionRule = new ProductionRule(Symbols["Constant"], productionRuleBody, 0, SemanticLibrary.AcReduceString);
            productionRules.Add(productionRule);

            //Formals -> Variable
            //         | (Variable_list)
            productionRuleBody = new List<Symbol> { Symbols["Variable"] };
            productionRule = new ProductionRule(Symbols["Formals"], productionRuleBody);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["Variable_list"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Formals"], productionRuleBody);
            productionRules.Add(productionRule);

            //Application -> (Expression Expression_list)
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["Expression"], Symbols["Expression_list"], Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Application"], productionRuleBody, 0, SemanticLibrary.AcBuildApplication);
            productionRules.Add(productionRule);

            //Datum -> Constant
            //       | (Datum_list)
            productionRuleBody = new List<Symbol> { Symbols["Constant"] };
            productionRule = new ProductionRule(Symbols["Datum"], productionRuleBody, 0, SemanticLibrary.AcRepeater );
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["(ParentOpen)"], Symbols["Datum_list"],Symbols["(ParentClose)"] };
            productionRule = new ProductionRule(Symbols["Datum"], productionRuleBody, 0, SemanticLibrary.AcRepeaterIgnoreFirst);
            productionRules.Add(productionRule);

            //Datum_list -> Datum Datum_list | EPS
            productionRuleBody = new List<Symbol> { Symbols["Datum"], Symbols["Datum_list"] };
            productionRule = new ProductionRule(Symbols["Datum_list"], productionRuleBody, 0, SemanticLibrary.AcBuildRightList);
            productionRules.Add(productionRule);
            productionRuleBody = new List<Symbol> { Symbols["Epsilon"] };
            productionRule = new ProductionRule(Symbols["Datum_list"], productionRuleBody, 0, SemanticLibrary.AcCreateList);
            productionRules.Add(productionRule);
        }
    }
}
