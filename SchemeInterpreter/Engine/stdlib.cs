using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Engine
{
    public static class Stdlib
    {
        public enum SchemeTypes
        {
            Boolean = 0,
            Number = 1,
            String = 2,
            Function = 3,
            List = 4,

            Primary = 5, //not really a datatype
            State = 6
        };

    }
}
