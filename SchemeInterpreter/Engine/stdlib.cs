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
            Variable = 8,
            Application = 9,
            Scope = 10,
            Lambda = 11
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

        //Type Equality
        public static Tuple<SchemeTypes, object> Eq(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if (list.Count != 2)
                throw new Exception("Wrong number of arguments for operator 'Eq?'. Expected 2, has:" + list.Count);

            return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, list[0].Item1 == list[1].Item1);
        }
        //Full Equality
        public static Tuple<SchemeTypes, object> Equal(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if(list.Count != 2)
                throw new Exception("Wrong number of arguments for operator '='. Expected 2, has:" + list.Count);
            
            if(list[0].Item1 != list[1].Item1)
                return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, false);

            //compare types
            switch (list[0].Item1)
            {
                case SchemeTypes.Boolean:
                    return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, (bool)list[0].Item2 == (bool)list[1].Item2);
                case SchemeTypes.Number:
                    const double eps = 0.00000001;
                    return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, (Math.Abs((double)list[0].Item2 - (double)list[1].Item2)) <= eps);
                case SchemeTypes.String:
                    return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, (string)list[0].Item2 == (string)list[1].Item2);
                case SchemeTypes.Function:
                    return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, (string)list[0].Item2 == (string)list[1].Item2);
                case SchemeTypes.List:
                    var listA = list[0].Item2 as List<Tuple<SchemeTypes, object>>;
                    var listB = list[0].Item2 as List<Tuple<SchemeTypes, object>>;

                    if(listA.Count != listB.Count)
                        return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, false); //different length
                    for (var i = 0; i < listA.Count; i++)
                    {
                        var eqTest = Equal(new List<Tuple<SchemeTypes, object>>() {listA[i], listB[i]});
                        if(!(bool)eqTest.Item2)
                            return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, false); //different objects 

                    }
                    return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, true);
                default:
                    throw new ArgumentOutOfRangeException("Type: "+ list[0].Item1 + " Is not comparable");
            }
        }

        //Greater or equal
        public static Tuple<SchemeTypes, object> GreaterOrEqual(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if (list.Count != 2)
                throw new Exception("Wrong number of arguments for operator '>='. Expected 2, has:" + list.Count);

            //cast check
            if(list[0].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' >= ' :: operand: "+list[0].Item2 +" is not a number");
            if(list[1].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' >= ' :: operand: " + list[1].Item2 + " is not a number");

            //return check
            if ((bool)Greater(args).Item2 || (bool)Equal(args).Item2)
                return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, true);
            return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, false);
        }
        //Less than or equal
        public static Tuple<SchemeTypes, object> LessOrEqual(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if (list.Count != 2)
                throw new Exception("Wrong number of arguments for operator '>='. Expected 2, has:" + list.Count);

            //cast check
            if (list[0].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' <= ' :: operand: " + list[0].Item2 + " is not a number");
            if (list[1].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' <= ' :: operand: " + list[1].Item2 + " is not a number");

            //return check
            if((bool)Lesser(args).Item2 || (bool)Equal(args).Item2)
                return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, true);
            return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, false);
        }

        //Greater
        public static Tuple<SchemeTypes, object> Greater(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if (list.Count != 2)
                throw new Exception("Wrong number of arguments for operator '>='. Expected 2, has:" + list.Count);

            //cast check
            if (list[0].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' > ' :: operand: " + list[0].Item2 + " is not a number");
            if (list[1].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' > ' :: operand: " + list[1].Item2 + " is not a number");

            return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, ((double)list[0].Item2 > (double)list[1].Item2));
        }
        //Lesser
        public static Tuple<SchemeTypes, object> Lesser(object args)
        {
            var list = args as List<Tuple<SchemeTypes, object>>;
            if (list.Count != 2)
                throw new Exception("Wrong number of arguments for operator '>='. Expected 2, has:" + list.Count);

            //cast check
            if (list[0].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' < ' :: operand: " + list[0].Item2 + " is not a number");
            if (list[1].Item1 != SchemeTypes.Number)
                throw new Exception("Operator ' < ' :: operand: " + list[1].Item2 + " is not a number");

            return new Tuple<SchemeTypes, object>(SchemeTypes.Boolean, ((double)list[0].Item2 < (double)list[1].Item2));
        }
    }
}
