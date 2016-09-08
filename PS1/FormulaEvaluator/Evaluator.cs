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
                if (Regex.Match(token, @"^[+\-//*///]$").Success)
                {
                    //evaluate addition and subtraction
                    if (token.Equals("+") || token.Equals("-"))
                    {
                        if (operators.Count >= 1 && values.Count >= 2)
                        {
                            if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                            {
                                int val1 = values.Pop();
                                int val2 = values.Pop();
                                values.Push(
                                    operators.Pop().PerformOperation(
                                        val2, val1));
                            }
                        }
                        operators.Push(token);
                    }
                    else
                    {
                        operators.Push(token);
                    }
                }
                
                //opening parenthesis
                else if (token.Equals("("))
                {
                    operators.Push(token);
                }

                //logic for finishing up parenthesis
                else if(token.Equals(")"))
                {
                    //final operations inside the parenthesis
                    if (operators.Count >= 1 && values.Count >= 2)
                    {
                        if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                        {
                            int val1 = values.Pop();
                            int val2 = values.Pop();
                            values.Push(
                                operators.Pop().PerformOperation(
                                    val2, val1));
                        }
                    }

                    if (!operators.Pop().Equals("(")) { throw new Exception("Invalid Expression"); }

                    //if there's a multiply or divide right before the parenthesis
                    if (operators.Count >= 1)
                    {
                        if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                        {
                            if (values.Count >= 2)
                            {
                                int val1 = values.Pop();
                                int val2 = values.Pop();
                                values.Push(operators.Pop().PerformOperation(val2, val1));
                            }
                            else
                            {
                                throw new Exception("Invalid Expression");
                            }
                        }
                    }

                }

                //At this point we either have a variable or an integer, or some other invalid character
                else if (!String.IsNullOrWhiteSpace(token))
                {
                    //should guarantee a value for operand, or a thrown exception within the delegate
                    int operand;
                    if (!int.TryParse(token, out operand))
                    {
                        operand = variableEvaluator(token);
                    }

                    //perform multiplication or division
                    if (operators.Count >= 1)
                    {
                        if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                        {
                            if (values.Count >= 1 && operators.Count >= 1)
                            {
                                values.Push(operators.Pop().PerformOperation(values.Pop(), operand));
                            }
                            else
                            {
                                throw new Exception("Invalid Expression"); 
                            }
                        }
                        else { values.Push(operand); } 
                    }
                    //should only be reached if the stack is empty, this is the first number
                    else { values.Push(operand); }
                }
            }

            //final operations once all tokens are processed
            if (operators.Count == 0 && values.Count == 1)
            {
                return values.Pop();
            }
            else if (operators.Count >= 1 && values.Count >= 2)
            {
                if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                {
                    int val1 = values.Pop();
                    int val2 = values.Pop();
                    return operators.Pop().PerformOperation(val2, val1);
                }
            }
            else
            {
                throw new Exception("Invalid Expression");
            }
            //should never get here
            return -1;
        }
    }

    /// <summary>
    /// Extends the String class to allow for more elegant arithmetic operations
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Performs addition, subtraction, multiplication and division on two integers
        /// </summary>
        /// <param name="token">An arithmetic operator</param>
        /// <param name="x">operand</param>
        /// <param name="y">operand</param>
        /// <returns>The result of the operation</returns>
        public static int PerformOperation(this string token, int x, int y)
        {
            try
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
            catch (DivideByZeroException)
            {
                throw new Exception("Invalid Operation: Divide by Zero");
            }
        }
    }
}
