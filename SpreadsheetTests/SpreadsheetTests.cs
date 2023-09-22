
using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;
using System.Linq;
using System.Threading.Channels;
using System.Xml;

namespace SpreadsheetTests
{


    /// <summary>
    ///This is a test class for SpreadsheetTest to test the functionality of the updated Spreadsheet
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {


        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidNameDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1A1A", "1.5");
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSimpleSetDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "1.5");
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetSimpleStringInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "hello");
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSetGetSimpleString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }


        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSetandReSet()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "10");
            s.SetContentsOfCell("A2", "=A1 * 2");
            s.SetContentsOfCell("A1", "2");
            Assert.AreEqual(4.0, s.GetCellValue("A2"));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetSimpleFormulaInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "=2");
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSetGetFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "=3");
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(new Formula("3"), f);
            Assert.AreNotEqual(new Formula("2"), f);
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [TestCategory("15")]
        [ExpectedException(typeof(CircularException))]
        public void TestSimpleCircularException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A1");
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void TestComplexCircular()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A3", "2");
            s.SetContentsOfCell("A5", ("=A5+A7"));
            s.SetContentsOfCell("A7", ("=A1+A1"));
            s.SetContentsOfCell("A1", ("=A5+A3"));

        }

        /// <summary>
        /// Testing to create a circular exception, and then get rid of the exception
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void TestUndoCircular()
        {
            Spreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A2", "15");
                s.SetContentsOfCell("A3", "30");
                s.SetContentsOfCell("A1", ("=A1+A3"));
                s.SetContentsOfCell("A2", ("=A2+A3"));
            }
            catch (CircularException e)
            {
                Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                throw e;
            }
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestEmptyNames()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestEmptyCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSimpleGetNameString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSimpleNamesDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "52.25");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestSimpleNamesFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", ("=3.5"));
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestGetMixedContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("B1", ("=3.5"));
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestGetCellContentFormula()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("C1", "3");
            s.SetContentsOfCell("A1", "=17 + C1");
            Assert.AreEqual(new Formula("17 + C1"), s.GetCellContents("A1"));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestinvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("_231dsajdw", "17.2");
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestChangeFormulatoString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", ("=3"));
            s.SetContentsOfCell("A1", "Hello");
            Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestChangeStringtoFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A1", ("=23"));
            Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
            Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));
        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestGetSavedVersion()
        {

            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3");
            s.SetContentsOfCell("A2", "4");
            s.SetContentsOfCell("A3", "5");
            s.Save("test2.txt");
            Assert.AreEqual("default", s.GetSavedVersion("test2.txt"));
        }


        /// <summary>
        /// testing when trying to look for the wrong version name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestWrongVersion()
        {
            Spreadsheet s = new Spreadsheet("test2.txt", s => true, s => s, "23");

        }

        /// <summary>
        /// See Title
        /// </summary>
        [TestMethod()]
        public void TestFourArgumentConstructor()
        {
            Spreadsheet s = new Spreadsheet("test2.txt", s => true, s => s, "default");
            Assert.AreEqual(false, s.Changed);

        }

        /// <summary>
        /// Testing when theres not the correct start tag for name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestWrongNameStartTag()
        {

            WrongSaveNameStartTag("wrongFile.txt");

            Spreadsheet w = new Spreadsheet("wrongFile.txt", s => true, s => s, "default");
        }


        /// <summary>
        /// private helper method for the above test to create the wrong file
        /// </summary>
        /// <param name="filename"> the file</param>
        private void WrongSaveNameStartTag(string filename)
        {

            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "43");
            s.SetContentsOfCell("D3", "54");
            // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", s.Version);

                foreach (string name in s.GetNamesOfAllNonemptyCells())
                {
                    writer.WriteStartElement("cell");
                    writer.WriteStartElement("notName");
                    writer.WriteString(name.ToString());
                    writer.WriteStartElement("notName");
                    writer.WriteString(s.GetCellContents(name).ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
        }

        /// <summary>
        /// Testing when theres not the correct end tag for name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestWrongNameEndTag()
        {

            WrongSaveNameEndTag("wrongFile.txt");

            Spreadsheet w = new Spreadsheet("wrongFile.txt", s => true, s => s, "default");
        }



        /// <summary>
        /// private helper method for the above test to create the wrong file
        /// </summary>
        /// <param name="filename"> the file</param>
        private void WrongSaveNameEndTag(string filename)
        {

            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "43");
            s.SetContentsOfCell("D3", "54");
            // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", s.Version);

                foreach (string name in s.GetNamesOfAllNonemptyCells())
                {

                    writer.WriteStartElement("cell");
                    writer.WriteStartElement("name");
                    writer.WriteString(name.ToString());
                    writer.WriteStartElement("notNontent");
                    writer.WriteString(s.GetCellContents(name).ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
        }


        /// <summary>
        /// testing when there is not a content starting tag
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestWrongContentStartTag()
        {

            WrongSaveContentStartTag("wrongFile.txt");

            Spreadsheet w = new Spreadsheet("wrongFile.txt", s => true, s => s, "default");
        }



        /// <summary>
        /// private helper method for the above test to create the wrong file
        /// </summary>
        /// <param name="filename"> the file</param>
        private void WrongSaveContentStartTag(string filename)
        {

            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "43");
            s.SetContentsOfCell("D3", "54");
            // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", s.Version);

                foreach (string name in s.GetNamesOfAllNonemptyCells())
                {

                    writer.WriteStartElement("cell");
                    writer.WriteStartElement("name");
                    writer.WriteString(name.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("notContent");
                    writer.WriteString(s.GetCellContents(name).ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
        }


        /// <summary>
        /// testing when there is not a correct end tag for content
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestWrongContentEndTag()
        {

            WrongSaveContentEndTag("wrongFile.txt");

            Spreadsheet w = new Spreadsheet("wrongFile.txt", s => true, s => s, "default");
        }



        /// <summary>
        /// private helper method for the above test to create the wrong file
        /// </summary>
        /// <param name="filename"> the file</param>
        private void WrongSaveContentEndTag(string filename)
        {

            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "43");
            s.SetContentsOfCell("D3", "54");
            // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", s.Version);

                foreach (string name in s.GetNamesOfAllNonemptyCells())
                {

                    writer.WriteStartElement("cell");
                    writer.WriteStartElement("name");
                    writer.WriteString(name.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("content");
                    writer.WriteString(s.GetCellContents(name).ToString());
                    writer.WriteStartElement("notContent");
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
        }

        /// <summary>
        /// testing when there is not a correct end tag for cell
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestWrongCellEndTag()
        {

            WrongSaveCellEndTag("wrongFile.txt");

            Spreadsheet w = new Spreadsheet("wrongFile.txt", s => true, s => s, "default");
        }



        /// <summary>
        /// private helper method for the above test to create the wrong file
        /// </summary>
        /// <param name="filename"> the file</param>
        private void WrongSaveCellEndTag(string filename)
        {

            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "43");
            s.SetContentsOfCell("D3", "54");
            // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", s.Version);

                foreach (string name in s.GetNamesOfAllNonemptyCells())
                {

                    writer.WriteStartElement("cell");
                    writer.WriteStartElement("name");
                    writer.WriteString(name.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("content");
                    writer.WriteString(s.GetCellContents(name).ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("notCell");

                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
        }

        /// <summary>
        /// Testing when there is not a correct end tag for the spreadsheet tag
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestWrongSpreadSheetEndTag()
        {

            WrongSaveSpreadSheetEndTag("wrongFile.txt");

            Spreadsheet w = new Spreadsheet("wrongFile.txt", s => true, s => s, "default");
        }



        /// <summary>
        /// private helper method for the above test to create the wrong file
        /// </summary>
        /// <param name="filename"> the file</param>
        private void WrongSaveSpreadSheetEndTag(string filename)
        {

            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "43");
            s.SetContentsOfCell("D3", "54");
            // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", s.Version);

                foreach (string name in s.GetNamesOfAllNonemptyCells())
                {

                    writer.WriteStartElement("cell");
                    writer.WriteStartElement("name");
                    writer.WriteString(name.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("content");
                    writer.WriteString(s.GetCellContents(name).ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteStartElement("notSpreadsheet");

                }


                writer.WriteEndDocument();

            }
        }

    }

}



