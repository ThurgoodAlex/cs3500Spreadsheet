using System.Text.RegularExpressions;
namespace FormulaEvaluator
{
    /// <summary>
    /// Author:    Alex Thurgood
    /// Partner:   None
    /// Date:      1/12/23
    /// Course:    CS 3500, University of Utah, School of Computing
    /// Copyright: CS 3500 and Alex Thurgood - This work may not 
    ///            be copied for use in Academic Coursework.
    ///
    /// I, Alex Thurgood, certify that I wrote this code from scratch and
    /// did not copy it in part or whole from another source.  All 
    /// references used in the completion of the assignments are cited 
    /// in my README file.
    ///
    /// File Contents
    ///
    ///    The Spreadsheet program is currently capable of being able being able to evaluate arithmetic expressions using 
    ///    non-negative integers, variables, and operators including the '(','+','-','*','/',)' operators.
    ///    This works because in my code, there are several different 'checks' that look at the current 
    ///    token and depending on what kind of token it is, there are specific actions tied to each kind of token.
    ///    
    /// </summary>

    public static class Evaluator

    {
        /// <summary>
        /// Currently this Lookup method evaluates the given variable to a set value. 
        /// This is mainly used in my testing method when using lambda expressions. 
        /// </summary>
        /// <param name="variable_name"> the variable that is evaluated to a value </param>
        /// <returns> the set value from the variable</returns>
        public delegate int Lookup(String variable_name);


        /// <summary>
        /// This method builds an algorithm to evaluate expressions 
        /// with the constraints of the operators being '+,-,/,*,(,)' while 
        /// using helper methods to express each operator and condition.
        /// </summary>
        /// 
        /// <param name="expression"> the formula to be evaluated </param>
        /// <param name="variableEvaluator"> provides an integer output for the provided variable</param>
        /// <returns> Returns the final number in the values stack </returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            // creating the value stack and the operator stack
            Stack<int> valuesStack = new();
            Stack<string> operatorsStack = new();

            // splitting the tokens by each operator
            string[] tokens = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string untrimmedToken in tokens)
            {
                // trims the leading and trailing white spaces after each token
                string token = untrimmedToken.Trim();

                // if current token is a space or empty string
                if ((token.Equals(" ")) || (token.Equals("")))
                {
                    continue;
                }
                // if current token is a '*' or '/'
                if (token.Equals("*") || token.Equals("/"))
                {
                    operatorsStack.Push(token);
                    continue;
                }
                // if current token is a '('
                if (token.Equals("("))
                {
                    operatorsStack.Push(token);
                    continue;
                }
                // if current token is an integer, it converts it out of a string and into an int format as "convertedToken"
                if (Int32.TryParse(token, out int convertedToken))
                {
                    intCondition(convertedToken, operatorsStack, valuesStack);
                    continue;
                }
                // if current token is a '+'
                if (token.Equals("+"))
                {
                    AdditionCondition(operatorsStack, valuesStack);

                    continue;
                }
                // if current token is a '-'
                if (token.Equals("-"))
                {
                    SubtractionCondition(operatorsStack, valuesStack);
                    continue;
                }

                // if current token is a ')'
                if (token.Equals(")"))
                {
                    rightParenthesisHelper(operatorsStack, valuesStack);
                    continue;
                }

                // current token is a properly named variable
                if (Regex.IsMatch(token, "^([a-z]|[A-Z])+\\d+$"))
                {
                    int varToken = variableEvaluator(token);
                    intCondition(varToken, operatorsStack, valuesStack);
                    continue;
                }

                // if the current token is not a compatible token
                else
                {
                    throw new ArgumentException();
                }
            }

            // ending condition when there are no more operators
            if (operatorsStack.Count == 0)
            {
                if(valuesStack.Count == 0)
                {
                    throw new ArgumentException();
                }
                else
                {
                    return valuesStack.Pop();
                }
            }

            else
            {
                // ending conditions when
                // there are exactly two values and a single operator left
                if (valuesStack.Count == 2 && operatorsStack.Count == 1)
                {
                    // if the last token on the operatorsStack is a '+'
                    if (operatorsStack.Peek().Equals("+"))
                    {
                        // pops the remaining two values off the stack and the last operator and evaluates
                        int poppednum1 = valuesStack.Pop();
                        int poppednum2 = valuesStack.Pop();
                        operatorsStack.Pop();
                        int sum = poppednum1 + poppednum2;
                        // pushes the sum back onto the valuesStack
                        valuesStack.Push(sum);


                    }
                    // if the last token on the operatorsStack is a '-'
                    else if (operatorsStack.Peek().Equals("-"))
                    {
                        // pops the remaining two values off the stack and the last operator and evaluates 
                        int poppednum1 = valuesStack.Pop();
                        int poppednum2 = valuesStack.Pop();
                        operatorsStack.Pop();
                        int sum = poppednum2 - poppednum1;
                        // pushes the sum back onto the valuesStack
                        valuesStack.Push(sum);

                    }
                }
                // when the ending expression is neither of the above cases
                else
                {
                    throw new ArgumentException();
                }
            }

            // pops the final number off the valuesStack as the final result of the expression
            return valuesStack.Pop();
        }

 
        /// <summary>
        /// This is the condition for when the current token is a '+' and the top of the operatorsStack is either a '+', '-', or neither
        /// </summary>
        /// 
        /// <param name="operatorsStack"> the stack that contains the operators </param>
        /// <param name="valuesStack">the stack that contains the values</param>
        /// <exception cref="ArgumentException"> this throws when the value stack has less than two values in it</exception>
        private static void AdditionCondition(Stack<string> operatorsStack, Stack<int> valuesStack)
        {

            //This checks the operators stack to see if there is any other operator in it, 
            //  If the stack is empty it pushes the '+'. 
            if (operatorsStack.Count == 0)
            {
                operatorsStack.Push("+");
            }
            // it checks to see if the top of the 
            // operators stack is either a '+' or '-'. If it is a '+', it checks if the valuesStack has less than two values and throws an exception if it true, 
            // if not it pops the top two values and applies the '+' operator. Then it pushes the sum to the valuesStack and pushes the '+' to the operatorsStack.
            else if (operatorsStack.Peek().Equals("+"))
            {
                if (valuesStack.Count < 2)
                {
                    throw new ArgumentException();
                }
                int poppednum1 = valuesStack.Pop();
                int poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                int sum = poppednum1 + poppednum2;
                valuesStack.Push(sum);
                operatorsStack.Push("+");

            }
            // it checks to see if the top of the 
            // operators stack is either a '+' or '-'. If it is a '-', it checks if the valuesStack has less than two values and throws an exception if it true, 
            // if not it pops the top two values and applies the '-' operator. Then it pushes the sum to the valuesStack and pushes the '-' to the operatorsStack.
            else if (operatorsStack.Peek().Equals("-"))
            {
                if (valuesStack.Count < 2)
                {
                    throw new ArgumentException();
                }
                int poppednum1 = valuesStack.Pop();
                int poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                int sum = poppednum2 - poppednum1;
                valuesStack.Push(sum);
                operatorsStack.Push("+");

            }
            // If the operatorsStack has neither on top, 
            // it pushes the '+' to the operatorsStack.
            else { operatorsStack.Push("+"); }

        }

        /// <summary>
        /// This is the condition for when the current token is a '-' 
        /// and there is either a '+', '-' or neither on top of the operatorsStack.
        /// </summary>
        /// 
        /// <param name="operatorsStack">the stack that contains the operators</param>
        /// <param name="valuesStack">the stack that contains the values</param>
        /// <exception cref="ArgumentException"> This throws when the values stack has less than two values</exception>
        private static void SubtractionCondition(Stack<string> operatorsStack, Stack<int> valuesStack)
        {
            //This checks the operators stack to see if there is any other operator in it, if there is other operators on top, 
            // it just pushes the '+' onto the stack. If the stack is empty it pushes the '-'.
            if (operatorsStack.Count == 0)
            {
                operatorsStack.Push("-");
            }
            // it checks to see if the top of the operators stack is either a '+' or '-'.  
            else if (operatorsStack.Peek().Equals("+"))
            {
                //it checks if the valuesStack has less than two values and throws an exception if it true,
                if (valuesStack.Count < 2)
                {
                    throw new ArgumentException();
                }
                // it pops the top two values and applies the '+' operator. Then it pushes the sum to the valuesStack and pushes the '-' to the operatorsStack.
                int poppednum1 = valuesStack.Pop();
                int poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                int sum = poppednum1 + poppednum2;
                valuesStack.Push(sum);
                operatorsStack.Push("+");

            }
            // it checks to see if the top of the operators stack is either a '+' or '-'. If it is a '-',  
            else if (operatorsStack.Peek().Equals("-"))
            {
                // it checks if the valuesStack has less than two values and throws an exception if it true,
                if (valuesStack.Count < 2)
                {
                    throw new ArgumentException();
                }
                //it pops the top two values and applies the '-' operator. Then it pushes the sum to the valuesStack and pushes the '+' to the operatorsStack.
                int poppednum1 = valuesStack.Pop();
                int poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                int sum = poppednum1 - poppednum2;
                valuesStack.Push(sum);
                operatorsStack.Push("-");

            }

            // If the operatorsStack has neither on top, 
            // it pushes the '-' to the operatorsStack.
            else { operatorsStack.Push("-"); }


        }

        /// <summary>
        /// This is the condition for when the current token is a non-negative integer, and either a '*', '/' or neither on top of the operators stack
        /// </summary>
        /// 
        /// <param name="convertedToken"> convertedToken is the current token but converted to an integer </param>
        /// <param name="operatorsStack"> the stack that contains the operators </param>
        /// <param name="valuesStack"> the stack that contains the values </param>
        /// <exception cref="ArgumentException"> throws this exception when the values stack has zero values in it, or when there is a division by zero</exception>
        private static void intCondition(int convertedToken, Stack<string> operatorsStack, Stack<int> valuesStack)
        {
            // It first checks the value stack to push immediately, 
            // but if not, it checks to see what is on top of the operatorsStack.
            if (valuesStack.Count == 0)
            {
                valuesStack.Push(convertedToken);
            }

            // If the top of the operatorsStack is a '*' 
            else if (operatorsStack.Peek().Equals("*"))
            {
                // it checks the valuesStack for a possible exception 
                if (valuesStack.Count == 0)
                {
                    throw new ArgumentException();
                }
                else
                {
                    // pops the '*' off the operatorsStack and the first number off the values stack and
                    // evaluates the popped number, the '*' and the current token and pushes the sum
                    operatorsStack.Pop();
                    int poppedNum = valuesStack.Pop();
                    int newValue = poppedNum * convertedToken;
                    valuesStack.Push(newValue);
                }


            }

            // If the top of the operatorsStack is a '/' 
            else if (operatorsStack.Peek().Equals("/"))
            {
                // it checks the valuesStack for a possible exception 
                if (valuesStack.Count == 0)
                {
                    throw new ArgumentException();
                }
                else
                {
                    operatorsStack.Pop();
                    int poppedNum = valuesStack.Pop();
                    // but it has one more check and that checks to see if the current token is a zero, if so it throws an exception for a division of zero error
                    if (convertedToken == 0)
                    {
                        throw new ArgumentException();
                    }
                    // if not it continues as normal and divided the popped number by the current token and pushes that new value. 
                    else
                    {
                        int sum = poppedNum / convertedToken;
                        valuesStack.Push(sum);
                    }
                }
            }
            // If the operatorsStack has neither of those two operators on top, the current token gets pushed to the valuesStack.
            else
            {
                valuesStack.Push(convertedToken);
            }

        }

        /// <summary>
        /// This is the condition for when the current token is a ')'.
        /// </summary>
        /// 
        /// <param name="operatorsStack"> the operators stack filled with all of the operators</param>
        /// <param name="valuesStack">the values stack filled with all of the values </param>
        /// <exception cref="ArgumentException">throws when theres less than two values in the values stack when the operatorsStack has a '+','-','/' or '*'. 
        /// When a'(' isn't found where expected when trying to pop the operators stack. Or when a division by zero happens.</exception>
        private static void rightParenthesisHelper(Stack<string> operatorsStack, Stack<int> valuesStack)
        {
            // checks to see if the operatorsStack has a '+' on top. if it does, it checks for the argument exception,
            if (operatorsStack.Peek().Equals("+"))
            {
                if (valuesStack.Count < 2)
                {
                    throw new ArgumentException();
                }
                // then pops the top two numbers off the value stack, pops the operators stack and then evaluates the
                // expression and pushes the sum back onto the values stack
                else
                {
                    int poppedNum1 = valuesStack.Pop();
                    int poppedNum2 = valuesStack.Pop();
                    operatorsStack.Pop();
                    int sum = poppedNum1 + poppedNum2;
                    valuesStack.Push(sum);

                }
            }
            // checks to see if the operatorsStack has a '-' on top. if it does, it checks for the argument exception,
            else if (operatorsStack.Peek().Equals("-"))
            {
                if (valuesStack.Count < 2)
                {
                    throw new ArgumentException();
                }
                // then pops the top two numbers off the value stack, pops the operators stack and then evaluates the
                // expression and pushes the sum back onto the values stack
                else
                {
                    int poppedNum1 = valuesStack.Pop();
                    int poppedNum2 = valuesStack.Pop();
                    operatorsStack.Pop();
                    int sum = poppedNum2 - poppedNum1;
                    valuesStack.Push(sum);

                }

            }
            //checks if the operators stack is empty, if it is it throws an exception,
            //if not it moves on to check what the top of the operatorsStack is 
            if (operatorsStack.Count == 0)
            {
                throw new ArgumentException();
            }
            // an exception is thrown if a '(' is not at the top of the operators stack
            if (!(operatorsStack.Peek().Equals("(")))
            {
                throw new ArgumentException();
            }

            // if a '(' is at the top it gets popped
            else { operatorsStack.Pop(); }

            if (!(operatorsStack.Count() == 0))
            {
                // if the operators stack isn't empty and theres a '*' at the top, 
                if (operatorsStack.Peek().Equals("*"))
                {
                    // checks if the values stack has less than two values and throws an exception 
                    if (valuesStack.Count < 2)
                    {
                        throw new ArgumentException();
                    }
                    // if it does have two values, it pops the operator, pops the top two values 
                    // and evaluates the two numbers and the operator and pushes that sum back onto the value stack
                    else
                    {
                        operatorsStack.Pop();
                        int poppedNum = valuesStack.Pop();
                        int poppedNum2 = valuesStack.Pop();
                        int newValue = poppedNum * poppedNum2;
                        valuesStack.Push(newValue);
                    }

                }
                // if the operators stack isn't empty and theres a '/' at the top, 
                else if (operatorsStack.Peek().Equals("/"))
                {
                    // checks if the values stack has less than two values and throws an exception 
                    if (valuesStack.Count < 2)
                    {
                        throw new ArgumentException();
                    }

                    // if it does have two values, it pops the operator, pops the top two values 
                    else
                    {
                        operatorsStack.Pop();
                        int poppedNum = valuesStack.Pop();
                        int poppedNum2 = valuesStack.Pop();
                        //when popping the second number, it checks for a possible division by zero and throws an error if the second number is a zero
                        if (poppedNum2 == 0)
                        {
                            throw new ArgumentException();
                        }
                        // evaluates the two numbers and the operator and pushes that sum back onto the value stack
                        else
                        {
                            int newValue = poppedNum / poppedNum2;
                            valuesStack.Push(newValue);
                        }
                    }

                }
            }
        }

    }
}
