using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluates functions
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Delegate to Look up the value for a variable
        /// </summary>
        /// <param name="v">Variable</param>
        /// <returns>Integer value of the variable</returns>
        public delegate int Lookup(String v);

        /// <summary>
        /// Evaluates a given expression
        /// </summary>
        /// <param name="exp">Expression as a string</param>
        /// <param name="variableEvaluator">Lookup Delegate</param>
        /// <returns>The result of the expression</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] tokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            Stack<string> operators = new Stack<string>();
            Stack<int> values = new Stack<int>();

            foreach(string token in tokens)
            {
                //I have previous experiences with regex stuff, so I promise I wrote this one myself :)
                if (Regex.Match(token, @"^[//+//\-//*///]$").Success)
                {

                    if (Regex.Match(token, @"^[//+//\-]$").Success)
                    {
                        if (Regex.Match(operators.Peek(), @"^[//+//\-]$").Success && values.Count >= 2)
                        {
                            values.Push(
                                operators.Pop().PerformOperation(
                                    values.Pop(), values.Pop()));
                        }
                        operators.Push(token);
                    }
                    else
                    {
                        operators.Push(token);
                    }
                }
                else if (token.Equals("("))
                {
                    operators.Push(token);
                }
                else if(token.Equals(")"))
                {
                    if (Regex.Match(operators.Peek(), @"^[//+//\-]$").Success && values.Count >= 2)
                    {
                        values.Push(
                            operators.Pop().PerformOperation(
                                values.Pop(), values.Pop()));
                    }

                    if (!operators.Pop().Equals("(")) { throw new Exception("Invalid Expression"); }

                    if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        if (values.Count >= 2 && operators.Count >= 2)
                        {
                            values.Push(operators.Pop().PerformOperation(values.Pop(), values.Pop()));
                        }
                        else
                        {
                            //throw an exception here? 
                        }
                    }

                }
                else
                {
                    int operand;
                    if (!int.TryParse(token, out operand))
                    {
                        operand = variableEvaluator(token);
                    }
                    if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        if (values.Count >= 1 && operators.Count >= 1)
                        {
                            values.Push(operators.Pop().PerformOperation(values.Pop(), operand));
                        }
                        else
                        {
                            //throw an exception here? 
                        }
                    }
                    else
                    {
                        values.Push(operand);
                    }
                }
            }

            //start by parsing the string into tokens
            //then look through tokens and evaluate for correctness
            //no need to go through twice
            //init two stacks
            //iterate through each token
            //check each token for what type it is (switch statement?)
            //evaluate all variables as we go?
            // TODO...
            return -1;
        }
    }



    //I got this idea off of stack overflow
    //http://stackoverflow.com/questions/7086058/convert-string-value-to-operator-in-c-sharp
    //I definitely am capable of doing this as a seperate method, but I wanted to 
    //practice extensions and make my algorithm prettier

    /// <summary>
    /// Extends the String class to allow for more elegant arithmetic operations
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token">An arithmetic operator</param>
        /// <param name="x">operand</param>
        /// <param name="y">operand</param>
        /// <returns>The result of the operation</returns>
        public static int PerformOperation(this string token, int x, int y)
        {
            switch (token)
            {
                case "*": return x * y;
                case "/": return x / y;
                case "+": return x + y;
                case "-": return x - y;
                default: throw new Exception("Token not a recognized operator");
            }
        }
    }
}
