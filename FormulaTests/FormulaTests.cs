using SpreadsheetUtilities;

namespace FormulaTests
{
    /// <summary>
    /// Author:    Alex Thurgood
    /// Partner:   None
    /// Date:      2/1/23
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
    /// This project tests all of the functionality from the Formula Project.
    /// 
    /// 
    ///    
    /// </summary>
    [TestClass]
    public class FormulaTests
    {

        /// <summary>
        /// testing an empty constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingEmptyConstructor()
        {
            Formula test = new Formula(" ");

        }

        /// <summary>
        /// testing a wrong first token, variable does not match the applied isValid rules
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingWrongTokenConstructor()
        {
            Formula test = new Formula("a12 + 1", s => s, simplevalid);

        }

        /// <summary>
        /// testing unbalancedParenthesisCount
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingUnBalancedParentheses()
        {
            Formula test = new Formula("(((()");

        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingRightParenthesesRule()
        {
            Formula test = new Formula("(())))");

        }

        /// <summary>
        /// Testing a case when the last variable does not match the isValid structure 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestingLastVariableWrong()
        {
            // NOTE: might not be right with the delegate, ask TA
            Formula test = new Formula("1 + hdsadad)!", s => s, simplevalid);

        }

        /// <summary>
        /// see title
        /// </summary>
        [TestMethod]
        public void TestingTokenGetCount()
        {
            Formula test = new Formula("1 + 1");
            Assert.AreEqual(2.0, test.Evaluate(s => 0));
            int tokenCount = test.getTokenCount();
            Assert.AreEqual(3, tokenCount);
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            Formula test = new Formula("x1234 + y1234 + z1234", s => s, simplevalid);
            Assert.AreEqual("x1234+y1234+z1234", test.ToString());
        }

        /// <summary>
        /// This tests that the GetVariables is properly working
        /// it uses an enumerator to move through the variables
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            Formula test = new Formula("1 + x3451", s => s, simplevalid);
            IEnumerator<string> e = test.GetVariables().GetEnumerator();


            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;

            Assert.IsTrue(s1 == "x3451");


        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestEquals()
        {
            Formula test = new Formula("1 + x3451", s => s, simplevalid);
            Formula test2 = new Formula("1 + x3451", s => s, simplevalid);

            Assert.IsTrue(test.Equals(test2));


        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestDoubleEquals()
        {
            Formula test = new Formula("1 + x3451", s => s, simplevalid);
            Formula test2 = new Formula("1 + x3451", s => s, simplevalid);

            Assert.IsTrue(test == test2);


        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestNotEquals()
        {
            Formula test = new Formula("1 + x3451", s => s, simplevalid);
            Formula test2 = new Formula("1 + x3452", s => s, simplevalid);

            Assert.IsTrue(test != test2);


        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod]
        public void TestGetHashcode()
        {
            Formula test = new Formula("1 + x3451", s => s, simplevalid);
            Formula test2 = new Formula("1 + x3451", s => s, simplevalid);

            Assert.IsTrue(test.Equals(test2));
            Assert.IsTrue(test.GetHashCode() == test2.GetHashCode());


        }

        /// <summary>
        /// Testing if an integer and a double are equal based on their ToString
        /// </summary>
        [TestMethod]
        public void TestIntvsDouble()
        {
            Formula test = new Formula("2", s => s, simplevalid);
            Formula test2 = new Formula("2.0", s => s, simplevalid);

            Assert.IsTrue(test.ToString().Equals(test2.ToString()));


        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSingleNumber()
        {
            Formula test = new Formula("5");
            Assert.AreEqual(5.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSingleVariable()
        {
            Formula test = new Formula("X5");
            Assert.AreEqual(13.0, test.Evaluate(s => 13));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestAddition()
        {
            Formula test = new Formula("5+3");
            Assert.AreEqual(8.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSubtraction()
        {
            Formula test = new Formula("18-10");
            Assert.AreEqual(8.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestMultiplication()
        {
            Formula test = new Formula("2*4");
            Assert.AreEqual(8.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestDivision()
        {
            Formula test = new Formula("16/2");
            Assert.AreEqual(8.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestArithmeticWithVariable()
        {
            Formula test = new Formula("2 +X1");
            Assert.AreEqual(6.0, test.Evaluate(s => 4));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestVarParenthMultiplication()
        {
            Formula test = new Formula("(X1 * 4)");
            Assert.AreEqual(16.0, test.Evaluate(s => 4));
        }


        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestScientificMatching()
        {
            Formula test = new Formula("5E2");
            Formula test2 = new Formula("500");
            Assert.IsTrue(test.Equals(test2));
        }


        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestScientificAddition()
        {
            Formula test = new Formula("5E2 + 4");
            Assert.AreEqual(504.0, test.Evaluate(s => 0));
        }



        /// <summary>
        /// testing if an unknown variable will get thrown and caught
        /// </summary>
        /// <exception cref="ArgumentException">It will throw an Argument Exception but when it catches it, it will return a formula error with a detailed message</exception>
        [TestMethod()]
        public void TestUnknownVariable()
        {
            Formula test = new Formula("2 + X1");

            Assert.AreEqual(new FormulaError("Please look at and reformat your formula, there is either an undefined variable or a divide by zero"), test.Evaluate(s => { throw new ArgumentException("Unknown variable"); }));
        }

        /// <summary>
        /// Testing to make sure that dividing by zero causes a formula error to be thrown
        /// </summary>
        [TestMethod()]
        public void TestDivideByZero()
        {
            Formula test = new Formula("5/0");
            Assert.AreEqual(new FormulaError("Please look at and reformat your formula, there is either an undefined variable or a divide by zero"), test.Evaluate(s => 0));
        }

        /// <summary>
        /// testing evaluating an expression left to right
        /// </summary>
        [TestMethod()]
        public void TestLeftToRight()
        {
            Formula test = new Formula("2*6+3");
            Assert.AreEqual(15.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// testing order of operations
        /// </summary>
        [TestMethod()]
        public void TestOrderOperations()
        {
            Formula test = new Formula("2+6*3");
            Assert.AreEqual(20.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestParenthesesTimes()
        {
            Formula test = new Formula("(2+6)*3", s => s, simplevalid);
            Assert.AreEqual(24.0, test.Evaluate(s => 0));
        }


        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestTimesParentheses()
        {
            Formula test = new Formula("2*(3+5)", s => s, simplevalid);
            Assert.AreEqual(16.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestPlusParentheses()
        {
            Formula test = new Formula("2+(3+5)", s => s, simplevalid);
            Assert.AreEqual(10.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestPlusComplex()
        {
            Formula test = new Formula("2+(3+5*9)", s => s, simplevalid);
            Assert.AreEqual(50.0, test.Evaluate(s => 0));
        }

        ///// <summary>
        ///// See Title
        ///// </summary>
        //[TestMethod()]
        //public void TestOperatorAfterParens()
        //{
        //    Formula test = new Formula("(1*1)-2/2", s => s, simplevalid);
        //    Assert.AreEqual(-0.5, test.Evaluate(s => 0));
        //}

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestComplexTimesParentheses()
        {
            Formula test = new Formula("2+3*(3+5)", s => s, simplevalid);
            Assert.AreEqual(26.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestComplexAndParentheses()
        {
            Formula test = new Formula("2+3*5+(3+4*8)*5+2", s => s, simplevalid);
            Assert.AreEqual(194.0, test.Evaluate(s => 0));
        }

        /// <summary>
        /// testing to make sure that a Formula Error gets thrown when its an incorrect formula
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSingleOperator()
        {
            Formula test = new Formula("+", s => s, simplevalid);
        }

        /// <summary>
        /// Making sure it throws when theres an extra operator
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraOperator()
        {
            Formula test = new Formula("2 + 5 +", s => s, simplevalid);
        }

        /// <summary>
        /// Making sure it throws when theres an extra parentheses 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("21")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraParentheses()
        {

            Formula test = new Formula("2+5*7)");

        }

        /// <summary>
        /// testing to make sure an invalid variable gets thrown
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidVariable()
        {
            Formula test = new Formula("xx", s => s, simplevalid);
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestPlusInvalidVariable()
        {
            Formula test = new Formula("5+xx", s => s, simplevalid);

        }

        /// <summary>
        /// tests to make sure that the Extra Following Rule works correctly, it throws because there is no operator after the ')'
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParensNoOperator()
        {
            Formula test = new Formula("5+7+(5)8", s => s, simplevalid);

        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmpty()
        {
            Formula test = new Formula("");

        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestComplexMultiVar()
        {
            Formula test = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.142857142857142, test.Evaluate(s => (s == "x7") ? 1 : 4));
        }

        /// <summary>
        /// making sure that all the pairs match
        /// </summary>
        [TestMethod()]
        public void TestComplexNestedParensRight()
        {
            Formula test = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6.0, test.Evaluate(s => 1));
        }

        /// <summary>
        /// making sure that all the pairs match
        /// </summary>
        [TestMethod()]
        public void TestComplexNestedParensLeft()
        {
            Formula test = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12.0, test.Evaluate(s => 2));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestRepeatedVar()
        {
            Formula test = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0.0, test.Evaluate(s => 3));
        }

        /// <summary>
        /// making sure that the Evaluate Stacks properly clear
        /// </summary>
        [TestMethod()]
        public void TestClearStacks()
        {
            //Test if code doesn't clear stacks between evaluations
            Formula test = new Formula("2*6+3");
            Assert.AreEqual(15.0, test.Evaluate(s => 0));
            Assert.AreEqual(15.0, test.Evaluate(s => 0));
        }




        /// <summary>
        /// A simple isValid method
        /// </summary>
        /// <param name="variable"> the variable to be looked at </param>
        /// <returns>a true if the the variable is correct based on the valid rules, false if not</returns>
        private Boolean simplevalid(string variable)
        {
            return variable.Length == 5;
        }
    }
}