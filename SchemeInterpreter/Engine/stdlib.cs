using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            //Non Terminal Datatypes
            Primary = 5,
            State = 6,
            Identifier = 7,
            Variable = 8
        };

        //Standard Library Functions

        //Addition
        public static Tuple<SchemeTypes, object> Addition(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            double result = 0;
            //Check for syntactic errors
            foreach (var element in list)
            {
                if(element.Item1 != SchemeTypes.Number)
                    throw new Exception("Element: "+ element.Item2 +" is not a number");
                result += (double) element.Item2;
            }

            return new Tuple<SchemeTypes, object>(SchemeTypes.Number, result);
        }

        //Divide
        public static Tuple<SchemeTypes, object> Division(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if(list[0].Item1 != SchemeTypes.Number)
                throw new Exception("Element: " + list[0].Item2 + " is not a number");
            var result = (double)list[0].Item2; //parse first element into initial div
            list.RemoveAt(0); //remove parsed element
            //Check for syntactic errors
            foreach (var element in list)
            {
                if (element.Item1 != SchemeTypes.Number)
                    throw new Exception("Element: " + element.Item2 + " is not a number");
                result /= (double)element.Item2;
            }

            return new Tuple<SchemeTypes, object>(SchemeTypes.Number, result);
        }

        //Multiply
        public static Tuple<SchemeTypes, object> Multiplication(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            double result = 1;
            //Check for syntactic errors
            foreach (var element in list)
            {
                if (element.Item1 != SchemeTypes.Number)
                    throw new Exception("Element: " + element.Item2 + " is not a number");
                result *= (double)element.Item2;
            }

            return new Tuple<SchemeTypes, object>(SchemeTypes.Number, result);
        }

        //Substract
        public static Tuple<SchemeTypes, object> Substraction(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if (list[0].Item1 != SchemeTypes.Number)
                throw new Exception("Element: " + list[0].Item2 + " is not a number");
            var result = (double)list[0].Item2; //parse first element into initial div
            list.RemoveAt(0); //remove parsed element
            //Check for syntactic errors
            foreach (var element in list)
            {
                if (element.Item1 != SchemeTypes.Number)
                    throw new Exception("Element: " + element.Item2 + " is not a number");
                result -= (double)element.Item2;
            }

            return new Tuple<SchemeTypes, object>(SchemeTypes.Number, result);
        }

        //display
        public static Tuple<SchemeTypes, object> Display(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            Console.WriteLine(list[0].Item2); //display item 2 to the STDOUT
            return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, false);
        }
    }
}
