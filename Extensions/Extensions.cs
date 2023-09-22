namespace Extensions
{
    /// <summary>
    /// Author:    Alex Thurgood
    /// Partner:   None
    /// Date:      2/2/23
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
    ///    This file contains most of my assignment 1 helper methods. This is to keep my main project folders less clustered 
    ///    and be able to clearly see these helper methods. These methods are the same from assignment 1 but have been 
    ///    slightly re factored to fit the assignment three details
    ///    
    /// </summary>
    public class Extensions

    {
        /// <summary>
        /// This is the condition for when the current token is a '+' and the top of the operatorsStack is either a '+', '-', or neither
        /// </summary>
        /// 
        /// <param name="operatorsStack"> the stack that contains the operators </param>
        /// <param name="valuesStack">the stack that contains the values</param>
        /// <exception cref="ArgumentException"> this throws when the value stack has less than two values in it</exception>
        public static void AdditionCondition(Stack<string> operatorsStack, Stack<double> valuesStack)
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
                double poppednum1 = valuesStack.Pop();
                double poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                double sum = poppednum1 + poppednum2;
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
                double poppednum1 = valuesStack.Pop();
                double poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                double sum = poppednum2 - poppednum1;
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
        public static void SubtractionCondition(Stack<string> operatorsStack, Stack<double> valuesStack)
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
                //if (valuesStack.Count < 2)
                //{
                //    throw new ArgumentException();
                //}
                // it pops the top two values and applies the '+' operator. Then it pushes the sum to the valuesStack and pushes the '-' to the operatorsStack.
                double poppednum1 = valuesStack.Pop();
                double poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                double sum = poppednum1 + poppednum2;
                valuesStack.Push(sum);
                operatorsStack.Push("+");

            }
            // it checks to see if the top of the operators stack is either a '+' or '-'. If it is a '-',  
            else if (operatorsStack.Peek().Equals("-"))
            {

                //it pops the top two values and applies the '-' operator. Then it pushes the sum to the valuesStack and pushes the '+' to the operatorsStack.
                double poppednum1 = valuesStack.Pop();
                double poppednum2 = valuesStack.Pop();
                operatorsStack.Pop();
                double sum = poppednum1 - poppednum2;
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
        public static void doubleCondition(double convertedToken, Stack<string> operatorsStack, Stack<double> valuesStack)
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

                // pops the '*' off the operatorsStack and the first number off the values stack and
                // evaluates the popped number, the '*' and the current token and pushes the sum
                operatorsStack.Pop();
                double poppedNum = valuesStack.Pop();
                double newValue = poppedNum * convertedToken;
                valuesStack.Push(newValue);



            }

            // If the top of the operatorsStack is a '/' 
            else if (operatorsStack.Peek().Equals("/"))
            {

                operatorsStack.Pop();
                double poppedNum = valuesStack.Pop();
                // but it has one more check and that checks to see if the current token is a zero, if so it throws an exception for a division of zero error
                if (convertedToken == 0)
                {
                    throw new ArgumentException();
                }
                //if (convertedToken - poppedNum == 0)
                //{
                //    throw new ArgumentException();
                //}
                // if not it continues as normal and divided the popped number by the current token and pushes that new value. 
                else
                {
                    double sum = poppedNum / convertedToken;
                    valuesStack.Push(sum);
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
        public static void rightParenthesisHelper(Stack<string> operatorsStack, Stack<double> valuesStack)
        {
            // checks to see if the operatorsStack has a '+' on top. if it does, it checks for the argument exception,
            if (operatorsStack.Peek().Equals("+"))
            {

                double poppedNum1 = valuesStack.Pop();
                double poppedNum2 = valuesStack.Pop();
                operatorsStack.Pop();
                double sum = poppedNum1 + poppedNum2;
                valuesStack.Push(sum);


            }
            // checks to see if the operatorsStack has a '-' on top. if it does, it checks for the argument exception,
            else if (operatorsStack.Peek().Equals("-"))
            {

                // then pops the top two numbers off the value stack, pops the operators stack and then evaluates the
                // expression and pushes the sum back onto the values stack

                double poppedNum1 = valuesStack.Pop();
                double poppedNum2 = valuesStack.Pop();
                double sum = poppedNum2 - poppedNum1;
                operatorsStack.Pop();
                if(sum == 0)
                {
                    throw new ArgumentException();
                }
                valuesStack.Push(sum);
                


            }
            //checks if the operators stack is empty, if it is it throws an exception,
            //if not it moves on to check what the top of the operatorsStack is 

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

                    // if it does have two values, it pops the operator, pops the top two values 
                    // and evaluates the two numbers and the operator and pushes that sum back onto the value stack


                    operatorsStack.Pop();
                    double poppedNum = valuesStack.Pop();
                    double poppedNum2 = valuesStack.Pop();
                    double newValue = poppedNum * poppedNum2;
                    valuesStack.Push(newValue);


                }
                // if the operators stack isn't empty and theres a '/' at the top, 
                else if (operatorsStack.Peek().Equals("/"))
                {


                    // if it does have two values, it pops the operator, pops the top two values 

                    operatorsStack.Pop();
                    double poppedNum = valuesStack.Pop();
                    double poppedNum2 = valuesStack.Pop();
                    //when popping the second number, it checks for a possible division by zero and throws an error if the second number is a zero
                    if (poppedNum2 == 0)
                    {
                        throw new ArgumentException();
                    }
                    // evaluates the two numbers and the operator and pushes that sum back onto the value stack
                    else
                    {
                        double newValue = poppedNum / poppedNum2;
                        valuesStack.Push(newValue);
                    }

                }
            }
        }
    }
}