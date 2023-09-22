namespace FormulaEvaluator;

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
///     This file includes all of my smaller testing methods and then a main method that calls those tester methods. 
///     There is an easy to read output screen that separates the functionality tests to the exception tests and outputs whether the test has succeeded or failed.
/// 
/// </summary>

class EvaluatorTester
{

    /// <summary>
    /// This method calls all of my smaller testing methods and outputs the results of each test 
    /// </summary>
    static void Main()
    {
        testSimpleMultiply();
        testSimpleDivide();
        testLargeMultiply();
        testSimpleAddition();
        testSimpleSubtraction();
        testSimpleParenthesisAddition();
        testSimpleParenthesisSubtraction();
        testSingleNumber();
        testDelegateMultiplication();
        testComplexExpression();
        testComplexVariableName();
        testVariableAddition();
        testDivideWithDecimals();
        testOnlyParenthesis();
        testComplexExpressionAndVariable();
        Console.WriteLine("\n Testing errors below \n");
        testDivideByZero();
        testNotTwoValuesInTheValuesStack();
        testDelegateImproperName();
        testNoOpeningParenthesis();
        testNotTwoValuesInTheValuesStackParenthesis();
        testNotACorrectExpression();
        testParenthesisDivideByZero();
        Console.Read();
    }

    /// <summary>
    /// testing a single number
    /// </summary>
    public static void testSingleNumber()
    {
        if (Evaluator.Evaluate("5", null) == 5) Console.WriteLine("Correct Answer for testSingleNumber");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing multiplication using variables
    /// </summary>
    public static void testDelegateMultiplication()
    {
        if (Evaluator.Evaluate("7 * A1", (x) => 5) == 35) Console.WriteLine("Correct Answer for testDelegateMultiplication!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing a unique variable name
    /// </summary>
    public static void testComplexVariableName()
    {
        if (Evaluator.Evaluate("5 + Aa12", (x) => 5) == 10) Console.WriteLine("Correct Answer for testComplexVariableName!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing addition using only variables
    /// </summary>
    public static void testVariableAddition()
    {
        if (Evaluator.Evaluate("B12 + Aa12", (x) => 7) == 14) Console.WriteLine("Correct Answer for testVariableAddition!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing an improper variable name 
    /// </summary>
    public static void testDelegateImproperName()
    {
        try
        {
            Evaluator.Evaluate(" 10 + R2D2", (x) => 5);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Correct Exception catch, improper variable name");
        }
    }

    /// <summary>
    /// testing simple multiplication
    /// </summary>
    public static void testSimpleMultiply()
    {

        if (Evaluator.Evaluate("5*5", null) == 25) Console.WriteLine("Correct Answer for testSimpleMultiply!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing large numbers while multiplying 
    /// </summary>
    public static void testLargeMultiply()
    {
        if (Evaluator.Evaluate("1000 * 10000", null) == 10000000) Console.WriteLine("Correct Answer for testLargeMultiply!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }

    }

    /// <summary>
    /// testing simple division
    /// </summary>
    public static void testSimpleDivide()
    {

        if (Evaluator.Evaluate("10/5", null) == 2) Console.WriteLine("Correct Answer for testSimpleDivide!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing division that comes up with an uneven number to make sure it rounds correctly 
    /// </summary>
    public static void testDivideWithDecimals()
    {

        if (Evaluator.Evaluate("12/5", null) == 2) Console.WriteLine("Correct Answer for testDivideWithDecimals!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing simple addition
    /// </summary>
    public static void testSimpleAddition()
    {

        if (Evaluator.Evaluate("5 + 5", null) == 10) Console.WriteLine("Correct Answer for testSimpleAddition!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing a complex expression
    /// </summary>
    public static void testComplexExpression()
    {
        if (Evaluator.Evaluate("(5 + 3) + 2 * 5", null) == 18) Console.WriteLine("Correct Answer for testComplexExpression!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing a division by zero exception catch
    /// </summary>
    public static void testDivideByZero()
    {
        try
        {
            Evaluator.Evaluate(" 10/0", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Correct Exception catch, cannot divide by zero");
        }
    }
    /// <summary>
    /// testing a division by zero exception catch but with parenthesis
    /// </summary>
    public static void testParenthesisDivideByZero()
    {
        try
        {
            Evaluator.Evaluate(" (10/0)", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Correct Exception catch, cannot divide by zero");
        }
    }

    /// <summary>
    /// testing exception when there are not two values in the value stack
    /// </summary>
    public static void testNotTwoValuesInTheValuesStack()
    {
        try
        {
            Evaluator.Evaluate("-5", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Correct Exception catch, not two values in the value stack");
        }
    }

    /// <summary>
    /// testing exception when there are not two values in the value stack but with parenthesis
    /// </summary>
    public static void testNotTwoValuesInTheValuesStackParenthesis()
    {
        try
        {
            Evaluator.Evaluate("(/6)", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Correct Exception catch, Value stack is empty");
        }
    }

    /// <summary>
    ///  testing exception when there is not an opening parenthesis
    /// </summary>
    public static void testNoOpeningParenthesis()
    {
        try
        {
            Evaluator.Evaluate(" 5*2+3)", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Correct Exception catch, There is not a '('");
        }
    }

    /// <summary>
    /// testing not a correct expression
    /// </summary>
    public static void testNotACorrectExpression()
    {
        try
        {
            Evaluator.Evaluate("5 5", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Correct Exception catch, This is not a correct token");
        }
    }

    /// <summary>
    /// testing simple subtraction
    /// </summary>
    public static void testSimpleSubtraction()
    {

        if (Evaluator.Evaluate("10 - 5", null) == 5) Console.WriteLine("Correct Answer for testSimpleSubtraction!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }   

    /// <summary>
    /// testing simple parenthesis addition
    /// </summary>
    public static void testSimpleParenthesisAddition()
    {
        if (Evaluator.Evaluate("(1 + 1)", null) == 2) Console.WriteLine("Correct Answer for testSimpleParenthesisAddition!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing simple parenthesis subtraction
    /// </summary>
    public static void testSimpleParenthesisSubtraction()
    {
        if (Evaluator.Evaluate("(2 - 1)", null) == 1) Console.WriteLine("Correct Answer for testSimpleParenthesisSubtraction!");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing single number with parenthesis
    /// </summary>
    public static void testOnlyParenthesis()
    {
        if (Evaluator.Evaluate("(1)", null) == 1) Console.WriteLine("Correct Answer for testOnlyParenthesis");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    /// <summary>
    /// testing complex expression with a variable
    /// </summary>
    public static void testComplexExpressionAndVariable()
    {
        if (Evaluator.Evaluate("(5 + 3) + A1 * 6", (x) => 5) == 38) Console.WriteLine("Correct Answer for testComplexExpressionAndVariable");
        else
        {
            Console.WriteLine("Wrong answer, please check your code");
        }
    }

    
}








