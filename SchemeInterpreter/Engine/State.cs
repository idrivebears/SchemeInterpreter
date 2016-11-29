using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchemeInterpreter.Structures;

namespace SchemeInterpreter.Engine
{
    public class State //no longer a struct
    {
        public readonly int StateId;
        public object Result;
        public readonly Symbol Primary;

        //For application
        public Func<object, object> Exec;
        public object Args;

        public State(int id)
        {
            StateId = id;
            Result = null;
            Primary = null;

            Exec = null;
            Args = null;
        }

        public State(int id, object result)
        {
            StateId = id;
            Result = result;
            Primary = null;

            Exec = null;
            Args = null;
        }

        public State(int id, Symbol primary)
        {
            StateId = id;
            Result = null;
            Primary = primary;

            Exec = null;
            Args = null;
        }

        public override string ToString()
        {
            if (Result != null)
                return @"<" + StateId + "> " + Result;
            return StateId.ToString();
        }
    }
}
