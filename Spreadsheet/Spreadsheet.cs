using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;


namespace SS
{
    /// <summary>
    /// Author:    Alex Thurgood
    /// Partner:   None
    /// Date:      2/14/23
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
    ///    
    /// This Spreadsheet project represents the state of a simple spreadsheet. This spreadsheet uses our Formula and DependencyGraph projects to create cells, 
    /// create dependencies between cells and add formulae as contents of a cell.
    /// 
    /// Assignment Five Additions:
    /// We can also read and create XML files. Some of the method headers and small details have been changed, 
    /// such as what a correct cell name should be. We are evaluating the cell only if the cell contains a formula
    /// 
    /// 
    /// 
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private DependencyGraph cellGraph;
        private Dictionary<string, Cell> nonEmptyCells;


        /// <inheritdoc/>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Creating an empty spreadsheet with a dependency graph for the cell relationships, and a dictionary to hold the non empty cells
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default")
        {
            cellGraph = new DependencyGraph();
            nonEmptyCells = new Dictionary<string, Cell>();
            Changed = false;
        }

        /// <summary>
        /// Creating an empty spreadsheet with a dependency graph for the cell relationships, and a dictionary to hold the non empty cells.
        /// This constructor also adds in an isValid, normalize and version. 
        /// </summary>
        /// <param name="isValid"> the isValid delegate given</param>
        /// <param name="normalize"> the Normalize delegate given</param>
        /// <param name="version"> the version given</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cellGraph = new DependencyGraph();
            nonEmptyCells = new Dictionary<string, Cell>();
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;
            Changed = false;

        }

        /// <summary>
        /// While doing everything the above constructors do, this is given a 
        /// filepath to read and check the correctness of the inputted XML file
        /// </summary>
        /// <param name="filePath"> the filepath given</param>
        /// <param name="isValid"> the isValid delegate given</param>
        /// <param name="normalize"> the Normalize delegate given</param>
        /// <param name="version"> the version given</param>
        /// <exception cref="SpreadsheetReadWriteException">If something goes wrong when reading, or if something is wrong with the format, or if anything else goes wrong    </exception>
        public Spreadsheet(String filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cellGraph = new DependencyGraph();
            nonEmptyCells = new Dictionary<string, Cell>();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            this.IsValid = isValid;
            this.Normalize = normalize;
            this.Version = version;
            string name = "";
            string content = "";

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath, settings))
                {

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    if (reader.GetAttribute("version") != version)
                                    {
                                        throw new SpreadsheetReadWriteException("there is not a version tag in the file");
                                    }
                                    break;
                                case "cell":
                                    reader.Read();
                                    if (reader.Name != "name")
                                    {
                                        throw new SpreadsheetReadWriteException("there is not a name tag in the file");
                                    }
                                    else
                                    {
                                        reader.Read();
                                        name = reader.Value;
                                    }
                                    reader.Read();
                                    if (reader.Name != "name")
                                    {
                                        throw new SpreadsheetReadWriteException("there is not a name tag in the file");
                                    }
                                    else
                                    {
                                        reader.Read();
                                    }
                                    if (reader.Name != "contents")
                                    {
                                        throw new SpreadsheetReadWriteException("There is no content tag in the file");
                                    }
                                    else
                                    {
                                        reader.Read();
                                        content = reader.Value;
                                    }
                                    reader.Read();
                                    if (reader.Name != "contents")
                                    {
                                        throw new SpreadsheetReadWriteException("There is no content tag in the file");
                                    }
                                    else
                                    {
                                        reader.Read();
                                    }
                                    if (reader.Name != "cell")
                                    {
                                        throw new SpreadsheetReadWriteException("there is a wrong end tag");
                                    }
                                    SetContentsOfCell(name, content);
                                    Changed = false;
                                    break;
                            }
                        }

                        else if (reader.Name != "spreadsheet")
                        {
                            throw new SpreadsheetReadWriteException("there is not a correct ending tag");
                        }
                        else
                        {
                            break;
                        }
                    }


                }

            }
            catch (SpreadsheetReadWriteException)
            {
                throw new SpreadsheetReadWriteException("Something is wrong with your formula");
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Invalid directory");
            }

            catch (XmlException)
            {
                throw new SpreadsheetReadWriteException("There is no file to read");
            }


        }

        /// <inheritdoc/>
        public override object GetCellContents(string name)
        {

            //checking the invalid name check and getting and returning the contents from the cell
            name = NameCheck(name);
            if (nonEmptyCells.TryGetValue(name, out Cell value) == false)
                return "";
            else
                return value.Contents;

        }

        /// <inheritdoc/>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {

            return nonEmptyCells.Keys;
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, double number)
        {

            // checking for exceptions
            NameCheck(name);
            // properly checking for duplicates or adding a new cell
            AddingACell(name, number);
            nonEmptyCells[name].Value = number;
            // replace the dependees with an empty HashSet because a text cant have dependees
            cellGraph.ReplaceDependees(name, new HashSet<string>());

            // returning all the names of the cells that are directly or indirectly dependent on the named cell and that need to be changed.
            IList<string> cellDependents = GetCellsToRecalculate(name).ToList();
            foreach (string nameOfCell in cellDependents)
            {
                object possibleCellToRecalcuulate = GetCellContents(nameOfCell);
                string PCRformula = "=" + possibleCellToRecalcuulate;
                if (possibleCellToRecalcuulate is Formula)
                {
                    SetContentsOfCell(nameOfCell, PCRformula);

                }

            }

            return cellDependents;
        }


        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, string text)
        {

            // properly checking for duplicates or adding a new cell
            AddingACell(name, text);
            nonEmptyCells[name].Value = text;
            // replace the dependees with an empty HashSet because a text cant have dependees
            cellGraph.ReplaceDependees(name, new HashSet<string>());
            // returning all the names of the cells that are directly or indirectly dependent on the named cell and that need to be changed.
            IList<string> cells = GetCellsToRecalculate(name).ToList();

            return cells;

        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {

            IList<string> cells;
            // getting the old dependees, replacing the current dependees with the variables from the formula
            IList<string> oldDependees = cellGraph.GetDependees(name).ToList();
            cellGraph.ReplaceDependees(name, formula.GetVariables());
            // try to call the GetCellsToRecalculate method and make sure that there aren't any circular exceptions
            try
            {
                cells = GetCellsToRecalculate(name).ToList();


            }
            catch
            {
                // replace the new dependees with the old ones if there is a circular exception
                cellGraph.ReplaceDependees(name, oldDependees);
                throw new CircularException();
            }

            // if there isn't a circular exception, call the CheckForDuplicates method to properly add a new cell or contents
            AddingACell(name, formula);
            // returning all the names of the cells that are directly or indirectly dependent on the named cell and that need to be changed.
            return cells;
        }

        /// <summary>
        /// Checks if the name does not pass the IsValid or the REGEX check or if the name is null
        /// </summary>
        /// <param name="name"> the name of the cell</param>
        /// <exception cref="InvalidNameException"> the exception is thrown for an invalid name </exception>
        private string NameCheck(string name)
        {

            name = Normalize(name);
            if (!IsValid(name))
            {
                throw new InvalidNameException();
            }
            Regex validNameRegex = new Regex("^[a-zA-Z]+[0-9]+$");
            if (name is null || !validNameRegex.Match(name).Success)
            {
                throw new InvalidNameException();
            }
            else
            {
                return name;
            }
        }


        /// <inheritdoc/>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            NameCheck(name);
            return cellGraph.GetDependents(name);
        }


        /// <summary>
        /// This helper method is checking for duplicates and if there is a duplicate name it sets current contents to the new contents.
        /// If there is not a duplicate, it creates a new cell with the contents.
        /// </summary>
        /// <param name="name"> the name of the cell</param>
        /// <param name="number"> the contents of the cell</param>
        private void AddingACell(string name, object contents)
        {
            if (nonEmptyCells.ContainsKey(name))
            {
                nonEmptyCells[name].Contents = contents;
            }
            else
            {
                nonEmptyCells.Add(name, new Cell(contents, ""));
            }

        }

        /// <inheritdoc/>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            IList<string> cellsToUpdate = new List<string>();
            try
            {
                name = NameCheck(name);
            }
            catch (InvalidNameException)
            {
                throw;
            }

            if (content is string)
            {
                // doesn't do anything if there is no content associated with the cell, simulating not setting a cell
                if (String.IsNullOrEmpty(content.ToString()))
                {
                    AddingACell(name, "");
                    SetCellContents(name, "");
                }

                // if the content is a double
                else if (Double.TryParse(content, out double value))
                {
                    SetCellContents(name, value);
                }

                // if the content is a formula
                else if (content[0].Equals('='))
                {

                    AddingACell(name, content);
                    Formula formula = new Formula(content.Substring(1), Normalize, IsValid);

                    // checking for a circular exception
                    try
                    {
                        cellsToUpdate = SetCellContents(name, formula);
                    }
                    catch
                    {
                        throw new CircularException();
                    }
                    // evaluating each cell when it gets added
                    foreach (string cellName in cellsToUpdate)
                    {
                        Cell cell = nonEmptyCells[cellName];
                        Formula newFormula = (Formula)nonEmptyCells[cellName].Contents;
                        try
                        {
                            cell.Value = newFormula.Evaluate(LookupValue);

                        }
                        catch (InvalidNameException)
                        {
                            throw new FormulaFormatException("There is an undefined variable due to the IsValid");
                        }
                    }
                }

                // if the content is just a string
                else
                {
                    string Stringcontent = content;
                    SetCellContents(name, Stringcontent);

                }

            }

            Changed = true;
            return cellsToUpdate;
        }

        /// <inheritdoc/>
        public override string GetSavedVersion(string filename)
        {

            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "spreadsheet":
                            return reader.GetAttribute(0).ToString();

                    }
                }
                throw new SpreadsheetReadWriteException("There is not a version tag");

            }

        }


        /// <inheritdoc/>
        public override void Save(string filename)
        {
            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            try
            {
                // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", this.Version);

                    foreach (string name in GetNamesOfAllNonemptyCells())
                    {
                        //this chunk of code properly formats what the XML file should look like
                        writer.WriteStartElement("cell");
                        writer.WriteStartElement("name");
                        writer.WriteString(name.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("contents");
                        if (nonEmptyCells[name].Contents is Formula)
                        {
                            writer.WriteString("=" + nonEmptyCells[name].Contents.ToString());
                        }
                        else
                        {
                            writer.WriteString(nonEmptyCells[name].Contents.ToString());
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("Missing file directory");
            }
            Changed = false;
        }

        /// <inheritdoc/>
        public override object GetCellValue(string name)
        {
            if (!nonEmptyCells.ContainsKey(name))
            {
                return "";
            }
            else
            {
                return nonEmptyCells[name].Value;
            }

        }


        /// <summary>
        /// THis is a wrapper method to return the cells value as a double
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">if the value cannot be turned into a double</exception>
        private double LookupValue(string name)
        {
            if (!IsValid(name))
            {
                throw new InvalidNameException();
            }
            object value = this.GetCellValue(name);
            if (value is double newValue)
            {
                return newValue;
            }
            else
            {
                throw new ArgumentException();
            }

        }


        /// <summary>
        /// a class that defines what an individual cell is
        /// </summary>
        private class Cell
        {

            /// <summary>
            /// a getter and setter for the cell contents and cell value
            /// </summary>
            public object Contents { get; set; }
            public object Value { get; set; }

            /// <summary>
            /// Sets the contents passed in as a parameter to the getter / setter
            /// </summary>
            /// <param name="contents"> The contents of the cell </param>
            /// <param name="value"> The value of the cell </param>
            public Cell(Object contents, object value)
            {

                this.Contents = contents;
                this.Value = value;
            }
        }
    }
}

