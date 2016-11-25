using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Engine
{
    public static class Enviroment
    {
        public static Dictionary<string, Dictionary<Stdlib.SchemeTypes, object>> TableSpace = new Dictionary<string, Dictionary<Stdlib.SchemeTypes, object>>();
    }
}
