using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace FormulaEvaluationTest
{
    /// <summary>
    /// Class to hold test cases
    /// </summary>
    class Program
    {
        /// <summary>
        /// Run test cases and display output
        /// </summary>
        /// <param name="args">Arguments from the command line</param>
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(Evaluator.Evaluate("10 / (6 / 3) / 2", VarTest));
                Console.WriteLine(Evaluator.Evaluate("4/2", VarTest));
                Console.WriteLine(Evaluator.Evaluate("0/2", VarTest));
                Console.WriteLine(Evaluator.Evaluate("A5+3", VarTest));
                Console.WriteLine(Evaluator.Evaluate("6*B6", VarTest)); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Sample delegate for looking up variables
        /// </summary>
        /// <param name="v">Variable to be looked up</param>
        /// <returns>The integer value of the variable</returns>
        private static int VarTest(string v)
        {
            Dictionary<string, int> variables = new Dictionary<string, int>();
            variables.Add("A5", 2);
            variables.Add("B6", 1);
            variables.Add("B7", 0);
            int value;
            if (!variables.TryGetValue(v, out value))
            {
                throw new Exception("Invalid Variable");
            }
            return value;
        }
    }
}
