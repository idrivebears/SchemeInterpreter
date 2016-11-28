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

        //AcRepeater -> Repeater action (retransmits state result_
        public static object AcRepeater(List<LR1Table.State> args)
        {
            return args[0].Result;
        }

        //AppendList -> * List
        public static object AcBuildRightList(List<LR1Table.State> args)
        {
            var node = args[0].Result as Tuple<Stdlib.SchemeTypes, object>; //catch new datum to add
            var list = args[1].Result as Tuple<Stdlib.SchemeTypes, object>;
            var listPointer = list.Item2 as List<Tuple<Stdlib.SchemeTypes, object>>;

            listPointer.Insert(0, node);

            return list; //return the list
        }
        //AppendList -> List *
        public static object AcBuildLeftList(List<LR1Table.State> args)
        {
            var node = args[1].Result as Tuple<Stdlib.SchemeTypes, object>; //catch new datum to add
            var list = args[0].Result as Tuple<Stdlib.SchemeTypes, List<Tuple<Stdlib.SchemeTypes, object>>>; //Catch the list variable
            list.Item2.Add(node); //Append the node to the list

            return list; //return the list
        }
        //Create Datum list
        public static object AcCreateList(List<LR1Table.State> args)
        {
            var newList = new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.List, new List<Tuple<Stdlib.SchemeTypes, object>>());
            return newList; //return the list
        }

        //AcReduceBoolean -> Reduces boolean to const
        public static object AcReduceBoolean(List<LR1Table.State> args)
        {
            var state = args[0];
            var primary = state.Primary as LR1Table.ExtendedSymbol;
            if(primary.Value == "true")
                return new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Boolean, true);
            return new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Boolean, false);
        }

        //AcReduceNumber -> Reduces Number to const
        public static object AcReduceNumber(List<LR1Table.State> args)
        {
            var state = args[0];
            var primary = state.Primary as LR1Table.ExtendedSymbol;
            var val = Convert.ToDouble(primary.Value);
            var result = new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Number, val);
            return result;
        }

        //AcReduceString -> Reduces string to const
        public static object AcReduceString(List<LR1Table.State> args)
        {
            var state = args[0];
            var primary = state.Primary as LR1Table.ExtendedSymbol;
            var val = primary.Value;
            var result = new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.String, val);
            return result;
        }

        //Ac 'Datum to Expression
        public static object AcRepeaterIgnoreFirst(List<LR1Table.State> args)
        {
            return args[1].Result;
        }
        //Application
        public static object AcExecuteApplication(List<LR1Table.State> args)
        {
            var function = args[1].Result as Tuple<Stdlib.SchemeTypes, object>;
            var stateArgs = args[2].Result as Tuple<Stdlib.SchemeTypes, object>;
            var funcArgs = stateArgs.Item2 as List<Tuple<Stdlib.SchemeTypes, object>>;

            Func<object, object> funcPtr;
            try
            {
                funcPtr = Enviroment.functions[(string) function.Item2]; //get function to execute
            }
            catch (Exception)
            {
                throw new Exception("Function: "+ function.Item2 + "is not defined");
            }

            return (funcPtr(funcArgs) as Tuple<Stdlib.SchemeTypes, object>);
        }

        //Build variable from identifier
        public static object AcBuildVariableFromId(List<LR1Table.State> args)
        {
            var primary = args[0].Primary as LR1Table.ExtendedSymbol;
            return new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Variable, primary.Value);
        }

        //Lookup variable in Enviroment
        public static object AcLookupSym(List<LR1Table.State> args)
        {
            var id = args[0].Result as Tuple<Stdlib.SchemeTypes, object>;
            Tuple<Stdlib.SchemeTypes, object> variable;

            try
            {
                variable = Enviroment.variables[(string) id.Item2];
            }
            catch (Exception)
            {
                throw new Exception("Symbol not found exception: "+ id.Item2);
            }
            //build result
            return variable;
        }
    }
}
