// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadsheetUtilities;




namespace SpreadsheetUtilities
{

    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>

    /// <summary>
    /// Author:    Alex Thurgood
    /// Partner:   None
    /// Date:      1/30/23
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
    /// This Formula project is a re factored version of assignment 1. We've changed how we deal with errors, now we throw a formula object.
    /// We check all of our syntax and making sure we have a valid formula when we construct it and not while we evaluate it. 
    /// The evaluate code is mostly the same from assignment 1 but slightly changed to fit the specifications of assignment 3. 
    /// 
    /// 
    /// See the Extensions Project for more information about the helper methods.
    /// 
    ///    
    /// </summary>
    public class Formula
    {

        List<string> variables = new List<string>();
        string newFormula = "";
        List<string> tokensList = new List<string>();
        int tokenCount = 0;
        int openingParen = 0;
        int closingParen = 0;

        HashSet<string> opList = new HashSet<string>
            {
                "*",
                "+",
                "-",
                "/"
            };



        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {

        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {

            string prevToken = "";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";


            // checks to see if the formula is empty 
            if (formula.Equals(" ") || formula.Equals(""))
            {
                throw new FormulaFormatException("Please look at and reformat your formula, there is either an undefined variable or a divide by zero");
            }

            foreach (string token in GetTokens(formula))
            {
                
                // if the token is an operator
                if (opList.Contains(token))
                {
                    tokensList.Add(token);
                    AddToFormula(token);
                    continue;
                }

                // if the token is a number
                if (Double.TryParse(token, out double newToken))
                {
                    tokensList.Add(token);
                    AddToFormula(newToken.ToString());
                    continue;
                }

                // if the token matches the variable REGEX
                if (Regex.IsMatch(token, varPattern))
                {
                    // if the variable does not pass the isValid and Normalize checks, it throws an error
                    if (!isValid(normalize(token)))
                    {

                        throw new FormulaFormatException("wrong");

                    }

                    // if it does pass, the variable gets added
                    else
                    {
                        if (variables.Contains(normalize(token)))
                        {
                            tokensList.Add(normalize(token));
                            AddToFormula(normalize(token));
                        }
                        else
                        {
                            tokensList.Add(normalize(token));
                            variables.Add(normalize(token));
                            AddToFormula(normalize(token));
                        }
                    }

                }

                // checks if the token is '(' or ')'
                if (token == "(" || token == ")")
                {
                    AddToFormula(token);
                }



                //sets prevToken to current token
                prevToken = token;


            }

            // Checking Starting Token Rule
            if (tokenCount == 1)
            {
                StartingTokenRule(normalize, isValid, tokensList);

            }

            // Checking the Parenthesis/Operator following rule
            FollowingRule(normalize, isValid, tokensList, opList);

            // Checking the Ending Token Rule
            EndingRule(normalize, isValid, tokensList);

            ExtraFollowingRule(normalize, isValid, tokensList);

            // checking Right Parentheses Rule and Unbalanced Parentheses Rule 
            if (closingParen > openingParen || openingParen != closingParen)
            {
                throw new FormulaFormatException("Incorrect format, please check your parenthesis pairs, you have more closing parenthesis than opening");
            }

        }

        /// <summary>
        /// This Method checks the extra following rule. This rule checks that any token immediately following a number, a variable or a closing parenthesis
        /// must either be an operator or a closing parenthesis
        /// </summary>
        /// <param name="normalize"> The normalizer that is passed in at definition time that normalizes all variables</param>
        /// <param name="isValid"> the validate function that determines what a valid variable is, passed in at definition time</param>
        /// <param name="tokensList"> the list of tokens</param>
        /// <returns> returns true if every case passes</returns>
        /// <exception cref="FormulaFormatException"> Throws an exception if there is a wrong token right after a number, a variable or a closing parenthesis</exception>
        private Boolean ExtraFollowingRule(Func<string, string> normalize, Func<string, bool> isValid, List<string> tokensList)
        {

            for (int i = 0; i < tokensList.Count - 1; i++)
            {
                // '(' check
                if (tokensList[i] == "(")
                {
                    continue;
                }
                // Operator check
                if (opList.Contains(tokensList[i]))
                {
                    continue;
                }

                // if the token is either a ')', number, or variable
                if (tokensList[i] == ")" || (Double.TryParse(tokensList[i], out double newPrevToken)) || isValid(normalize(tokensList[i])))
                {
                    // if token + 1 is ')'
                    if (tokensList[i + 1] == ")")
                    {
                        continue;
                    }

                    // if token + 1 is an operator
                    else if (opList.Contains(tokensList[i + 1]))
                    {
                        continue;
                    }

                    //throws an exception if its neither
                    else
                    {
                        throw new FormulaFormatException("Wrong ending token, please change your formula");
                    }
                }

            }
            return true;

        }

        /// <summary>
        /// This method checks the ending token rule where the last token of an expressions must be a number, a variable or a closing parenthesis
        /// </summary>
        /// <param name="normalize"> The normalizer that is passed in at definition time that normalizes all variables</param>
        /// <param name="isValid"> the validate function that determines what a valid variable is, passed in at definition time</param>
        /// <param name="tokensList"> the list of tokens</param>
        /// <returns> returns true if there is a correct ending token</returns>
        /// <exception cref="FormulaFormatException"> Throws an exception if there is a wrong ending token</exception>
        private Boolean EndingRule(Func<string, string> normalize, Func<string, bool> isValid, List<string> tokensList)
        {

            if (opList.Contains(tokensList.Last()))
            {
                throw new FormulaFormatException("wrong ending token, please reformat your formula");
            }
            // if the ending token is a number
            if (Double.TryParse(tokensList.Last(), out double convertedToken))
            {
                return true;
            }

            // if the ending token is a ')'
            if (tokensList.Last() == ")")
            {
                return true;
            }


            // if the ending token is a variable 
            if (isValid(normalize(tokensList.Last())))
            {

                return true;
            }
            // if its none of those cases

            else
            {
                throw new FormulaFormatException("wrong ending token, please reformat your formula");
            }
        }

        /// <summary>
        /// This method checks the Parenthesis/Operator Following Rule. This is where any token that immediately follows an opening parenthesis or an operator
        /// must be either a number, variable or an opening parenthesis
        /// </summary>
        /// <param name="normalize"> The normalizer that is passed in at definition time that normalizes all variables</param>
        /// <param name="isValid"> the validate function that determines what a valid variable is, passed in at definition time</param>
        /// <param name="tokensList"> the list of tokens</param>
        /// <param name="opList">the list of operators</param>
        /// <returns> returns true if every case passes</returns>       
        /// <exception cref="FormulaFormatException">Throws an exception if the following rule fails</exception>
        private Boolean FollowingRule(Func<string, string> normalize, Func<string, bool> isValid, List<string> tokensList, HashSet<string> opList)
        {
            for (int i = 0; i < tokensList.Count; i++)
            {
                // if the current token is a '('
                if (tokensList[i] == "(")
                {
                    // if the next token is '('
                    if (tokensList[i + 1] == "(")
                    {
                        return true;
                    }

                    // if the next token is a number 
                    if (Double.TryParse(tokensList[i + 1], out double convertedToken))
                    {
                        return true;
                    }

                    // if the next token is a variable
                    if (isValid(normalize(tokensList[i + 1])))
                    {

                        return true;
                    }

                    // if the next token is none of those cases
                    else
                    {
                        throw new FormulaFormatException("Your Formula is incorrect, please reformat");
                    }

                }

                // if the current token is an operator
                if (opList.Contains(tokensList[i]))
                {
                    if (opList.Contains(tokensList[i + 1]))
                    {
                        throw new FormulaFormatException("Your Formula is incorrect, please reformat");

                    }

                    // if the next token is '('
                    if (tokensList[i + 1] == "(")
                    {
                        return true;
                    }

                    // if the next token is a number 
                    if (Double.TryParse(tokensList[i + 1], out double convertedToken))
                    {
                        return true;
                    }

                    // if the next token is a variable
                    if (isValid(normalize(tokensList[i + 1])))
                    {

                        return true;

                    }

                    // if the next token is none of those cases
                    else
                    {
                        throw new FormulaFormatException("Your Formula is incorrect, please reformat");
                    }

                }

            }
            return true;

        }

        /// <summary>
        /// This method checks the Starting Token Rule, where the first token of an expression must be a number, a variable, or an opening parenthesis
        /// </summary>
        /// <param name="normalize"> The normalizer that is passed in at definition time that normalizes all variables</param>
        /// <param name="isValid"> the validate function that determines what a valid variable is, passed in at definition time</param>
        /// <param name="tokensList"> the list of tokens</param>
        /// <returns> returns true if every case passes</returns>       
        /// <exception cref="FormulaFormatException">Throws an exception if the Starting Token rule fails</exception>
        private Boolean StartingTokenRule(Func<string, string> normalize, Func<string, bool> isValid, List<string> tokenList)
        {

            if (opList.Contains(tokensList[0]))
            {
                throw new FormulaFormatException("You have the wrong token to start out your formula, please try a different starting token");
            }
            // if the starting token is a number
            if (Double.TryParse(tokenList[0], out double convertedToken))
            {
                return true;
            }

            // if the starting token is a '('
            if (tokenList[0] == "(")
            {
                return true;
            }

            // if the starting token is a variable
            if (isValid(normalize(tokenList[0])))
            {
                return true;

            }

            // if the starting token is none of those cases
            else
            {
                throw new FormulaFormatException("You have the wrong token to start out your formula, please try a different starting token");
            }

        }

        /// <summary>
        /// Looks at the token and depending if its a '(', ')' it either adds to the opening parenthesis count or closing parenthesis count.
        /// No matter what type of token it is, it adds to the new formula string and adds to the token count.
        /// </summary>
        /// <param name="token"> the current token</param>
        private void AddToFormula(string token)
        {
            // if token is a '('
            if (token == "(")
            {
                //adds to opening Parenthesis count,
                //adds to new formula string,
                //increases token count and adds the token to the tokens list
                openingParen++;
                newFormula += token;
                tokenCount++;
                tokensList.Add(token);

            }

            // if the token is a ')'
            else if (token == ")")
            {
                //adds to closing Parenthesis count,
                //adds to new formula string,
                //increases token count and adds the token to the tokens list
                closingParen++;
                newFormula += token;
                tokenCount++;
                tokensList.Add(token);

            }
            else
            {
                //adds to new formula string,
                //increases token count
                newFormula += token;
                tokenCount++;

            }

        }

        /// <summary>
        /// gets and returns the token count
        /// </summary>
        /// <returns>the token count</returns>
        public int getTokenCount()
        {
            return tokenCount;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            // creating the value stack and the operator stack
            Stack<double> valuesStack = new();
            Stack<string> operatorsStack = new();

            // splitting the tokens by each operator
            string[] tokens = Regex.Split(newFormula, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            try
            {
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
                    if (Double.TryParse(token, out double convertedToken))
                    {
                        Extensions.Extensions.doubleCondition(convertedToken, operatorsStack, valuesStack);
                        continue;
                    }
                    // if current token is a '+'
                    if (token.Equals("+"))
                    {
                        Extensions.Extensions.AdditionCondition(operatorsStack, valuesStack);
                        continue;
                    }
                    // if current token is a '-'
                    if (token.Equals("-"))
                    {
                        Extensions.Extensions.SubtractionCondition(operatorsStack, valuesStack);
                        continue;
                    }

                    // if current token is a ')'
                    if (token.Equals(")"))
                    {
                        Extensions.Extensions.rightParenthesisHelper(operatorsStack, valuesStack);
                        continue;
                    }

                    // current token is a properly named variable
                    else
                    {
                        try
                        {
                            double varToken = lookup(token);
                            Extensions.Extensions.doubleCondition(varToken, operatorsStack, valuesStack);
                            continue;
                        }
                        catch (FormulaFormatException)
                        {
                            return new FormulaError("There is an undefined variable");
                        }

                    }


                }

                // ending condition when there are no more operators
                if (operatorsStack.Count == 0)
                {

                    return valuesStack.Pop();

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
                            double poppednum1 = valuesStack.Pop();
                            double poppednum2 = valuesStack.Pop();
                            operatorsStack.Pop();
                            double sum = poppednum1 + poppednum2;
                            // pushes the sum back onto the valuesStack
                            valuesStack.Push(sum);


                        }
                        // if the last token on the operatorsStack is a '-'
                        else if (operatorsStack.Peek().Equals("-"))
                        {
                            // pops the remaining two values off the stack and the last operator and evaluates 
                            double poppednum1 = valuesStack.Pop();
                            double poppednum2 = valuesStack.Pop();
                            operatorsStack.Pop();
                            double sum = poppednum2 - poppednum1;
                            // pushes the sum back onto the valuesStack
                            valuesStack.Push(sum);

                        }
                    }

                }


                // pops the final number off the valuesStack as the final result of the expression
                return valuesStack.Pop();
            }
            catch (ArgumentException)
            {
                return new FormulaError("Please look at and reformat your formula, there is either an undefined variable or a divide by zero");
            }

        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        /// <returns> returns the ToString of the formula </returns>


        public override string ToString()
        {
            return newFormula;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        /// <returns> Returns true if the objects have equality, false if not. </returns>

        public override bool Equals(object? obj)
        {

            return obj.ToString().Equals(this.ToString());


        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        /// <returns> Returns true if the objects have equality, false if not. </returns>

        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        /// <returns> Returns true if the objects do not have equality, false if not. </returns>

        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);

        }


        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        /// <returns> Returns the objects hashcode </returns>

        public override int GetHashCode()
        {
            return ToString().GetHashCode();

        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>

        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {

                    yield return s;
                }

            }

        }


    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"> The reason for the error </param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }

}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
