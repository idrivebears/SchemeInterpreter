using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Engine
{
    public static class Enviroment
    {
        public static Dictionary<string, Func<object, object>> functions = new Dictionary
            <string, Func<object, object>>()
        {
            {"+", Stdlib.Addition},
            {"-", Stdlib.Substraction},
            {"/", Stdlib.Division},
            {"*", Stdlib.Multiplication},
            {"display", Stdlib.Display},
        };

        public static Dictionary<string, Tuple<Stdlib.SchemeTypes, object>> variables =
            new Dictionary<string, Tuple<Stdlib.SchemeTypes, object>>()
            {
                {"+", new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Function, "+")},
                {"-", new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Function, "-")},
                {"/", new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Function, "/")},
                {"*", new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Function, "*")},
                {"display", new Tuple<Stdlib.SchemeTypes, object>(Stdlib.SchemeTypes.Function, "display")},
            };
    }
}
