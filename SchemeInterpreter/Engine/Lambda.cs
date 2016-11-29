using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Engine
{
    class Lambda
    {
        //This is a lambda function
        public static Dictionary<string, Tuple<Stdlib.SchemeTypes, object>> Variables; //local scope
        
        public Lambda(Tuple<Stdlib.SchemeTypes, object> application)
        {
            //build lambda function from application
        }
    }
}
