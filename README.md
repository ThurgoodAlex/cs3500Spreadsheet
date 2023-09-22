```
Author:     Alex Thurgood
Partner:    None
Date:       12-Jan-2023
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  ThurgoodAlex
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-ThurgoodAlex
Date:       20-Feb-2023 11:00 am
Solution:   Spreadsheet
Copyright:  CS 3500 and Alex Thurgood - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of being able being able to evaluate arithmetic expressions using 
non-negative integers, variables, and operators such as the '(,+,-,*,/,)'.This works because in my code, I have implemented several different 'checks' 
that look at the current token and depending on what kind of token it is, there are specific actions tied to each kind of token.

I added the capabilities of being able to create a dependency graph between variables and cells. The project is able to add, remove, return if the dependents or dependees is empty or not,and replace the dependents and dependees. 
The methods in this project can also get the size of the dependency graph, the size of the dependents and dependees, and can return HashSet of dependents or dependees.

NOTE: see DependencyGraph's first summary for a detailed description about what dependents and dependees are.

This Formula project is a re factored version of assignment 1. We've changed how we deal with errors, now we throw a formula object.
We check all of our syntax and making sure we have a valid formula when we construct it and not while we evaluate it. The evaluate code is mostly the same from assignment 1 but slightly changed to fit the specifications of assignment 3. 

Note:  See the Extensions Project for more info about the helper methods.

The Spreadsheet project represents the state of a simple spreadsheet. This spreadsheet uses our Formula and DependencyGraph projects to create cells, 
create dependencies between cells and add formulae as contents of a cell.

    Assignment Five Additions:
     We can also read and create XML files. Some of the method headers and small details have been changed, 
     such as what a correct cell name should be. We are evaluating the cell only if the cell contains a formula
        

# Examples of Good Software Practice (GSP)

The first example on my solution that shows GSP is my use of DRY. For example, in the Spreadsheet project, I created helper methods for sections of code that I repeated multiple times. 
These helper methods include checking if the name of the cell is invalid / null and a check if the cell already contains the contents or if we need to create a new cell. 
These things happen in every SetCellContents method so it would have been redundant to not make helper methods.

Another example of GSP in my solution is my use of self documenting code. I try and make all of my variable names, method names, tester methods to be very specific to what they are designed to do.
This causes less confusion when reading my code and looking back at my code. This also lets there be less comments about what the code is doing because of the specific names.

I also started testing and commenting my code earlier on in the week and didn't wait until the last minute to do everything. When I was writing my tests earlier on, it allowed me to catch bugs earlier, and allowed me to fix them before I started writing more complex methods and tests.  
Writing my comments before hand helped my understanding of what each method has to accomplish and test my understanding of what complex code does. 

Assignment Five:

For this assignment, I did a good job of using regression and being able to reformat my old AS4 tests to fit AS5. 
This also helped me with better understanding what each method should output and helped me create more tests before I wrote my code.

I did a better job on this assignment with creating short private helper methods that helped consolidate my code. 
I was also able to add functionality from certain methods into my private helper methods so it became less repetitive. 

The above GSP helped improve my self-documenting code. I had to go back through my code and rename most of my private helper methods and some of the content in the newly added methods. 
THis allowed me to clearly see what all of my methods were clearly doing and really helped improve my understanding of my own code.


# Time Expenditures:

    1. Assignment One:   Predicted Hours:   15        Actual Hours:   21
    2. Assignment Two:   Predicted Hours:   20        Actual Hours:   11
    3. Assignment Three: Predicted Hours:   22        Actual Hours:   21.5
    4. Assignment Four:  Predicted Hours:   15        Actual Hours:   12
    4. Assignment Five:  Predicted Hours:   15        Actual Hours:   21
