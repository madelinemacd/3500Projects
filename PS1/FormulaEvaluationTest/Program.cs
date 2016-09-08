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
            Evaluator.Evaluate("4 + 2 * 6 / 3 - 2", VarTest);
        }

        /// <summary>
        /// Sample delegate for looking up variables
        /// </summary>
        /// <param name="v">Variable to be looked up</param>
        /// <returns>The integer value of the variable</returns>
        private static int VarTest(string v)
        {
            return -1;
        }
    }
}
