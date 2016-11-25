using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.SyntacticAnalysis;

namespace SchemeInterpreter.Engine
{
    public static class SemanticLibrary
    {
        //Semantic Action library ::

        //Action A0 -> Repeater action (retransmits state result_
        public static object ActionA0(List<LR1Table.State> args)
        {
            return args[0].Result;
        }
        //Action A1 -> Reduces boolean to const
        public static object ActionA1(List<LR1Table.State> args)
        {
            var state = args[0];
            var sym = state.Primary as LR1Table.ExtendedSymbol;
            Debug.Assert(sym != null, "sym != null");
            if(sym.Value == "true")
                return new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Boolean, true);
            else
                return new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Boolean, false);
        }

        // IfAction -> Self explanatory
        public static object IfAction(List<LR1Table.State> args)
        {
            Debug.Assert(args.Count == 2, "If action expects two args.");
            var condition = args[0];
            var branch = args[1];

        }
    }
}
