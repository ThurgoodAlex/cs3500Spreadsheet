// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)


namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>


    /// <summary>
    /// Author:    Alex Thurgood
    /// Partner:   None
    /// Date:      1/26/23
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
    ///    The DependencyGraph project is capable of creating ordered pairs in the form of (s,t) where both 's' and 't' are strings. When a dependency is added, 
    ///    a reverse dependee pair is also added in the form of (t,s). We are able to check the count of either the dependents or dependees, we can replace the 
    ///    dependents or dependees of one pair. We can also remove a dependency pair that will also get rid of the reverse dependee pair. 
    ///    We can also make 's' or 't' have multiple different dependents that they depend on.
    ///     
    ///    
    ///    
    /// </summary>

    public class DependencyGraph
    {
        // creates the backing storages and the size variable
        private int size;
        private Dictionary<string, HashSet<string>> dependees = new();
        private Dictionary<string, HashSet<string>> dependents = new();

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary
        /// >
        public DependencyGraph()
        {
            size = 0;
            dependees = new Dictionary<string, HashSet<string>>();
            dependents = new Dictionary<string, HashSet<string>>();

        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        /// <return> The number of ordered pairs in the dependency dictionary</return>
        public int Size
        {
            get
            {
                return this.size;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        /// <param name="s"> which dependee we are looking into </param>
        /// <return> The number of dependents 's' depends on</return>
        public int this[string s]
        {
            get
            {
                if (!dependees.ContainsKey(s))
                {
                    return 0;
                }

                return dependees[s].Count;
            }

        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        /// <param name="s"> which dependent we are looking into  </param>
        /// <returns> true if 's' has dependents, false if not or 's' does not exist </returns>
        public bool HasDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                if (!(dependents[s].Count == 0))
                {
                    return true;
                }
                else { return false; }
            }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        /// <param name="s"> the dependee which we are looking into </param>
        /// <returns> true if 's' has dependees, false if not or 's' does not exist </returns>
        public bool HasDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                if (!(dependees[s].Count == 0))
                {
                    return true;
                }
                else { return false; }

            }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        /// <param name="s">where we are getting the dependents from </param>
        /// <returns>returns the dependents of 's' if 's' exists. returns an empty Hashset if 's' is not in dependents</returns>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<string> result = new();

            if (dependents.ContainsKey(s))
            {
                return (dependents[s]);
            }
            return result;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        /// <param name="s">where we are getting the dependees from  </param>
        /// <returns> returns the dependees of 's' if 's' exists. returns an empty hashset if 's' is not in dependents</returns>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<string> result = new();
            if (dependees.ContainsKey(s))
            {
                return (dependees[s]);
            }
            return result;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>    
        public void AddDependency(string s, string t)
        {
            // if the dependent does not already contain 's'
            if (!(dependents.ContainsKey(s)))
            {
                // a new dependency pair is created with 's' as the dependent
                dependents.Add(s, new HashSet<string>());
                // then 't' gets added as the dependee
                dependents[s].Add(t);

            }
            // if dependents contains 's' but does not contain 't'
            if (!(dependents[s].Contains(t)))
            {
                // 't' gets added as the dependee to 's'
                dependents[s].Add(t);
            }

            // we are now concerned with adding the reverse dependee pair.
            // We are checking to see if 't' already exists in the dependees 
            if (!dependees.ContainsKey(t))
            {
                // we create a new dependee pair with 't' as the dependent
                dependees.Add(t, new HashSet<string>());

            }
            // if 't' exists and does not already contain
            if (!(dependees[t].Contains(s)))
            {
                // we then add 's' to 't'
                dependees[t].Add(s);
                // we only increment the size if we successfully create the dependency pair and the reverse dependee pair.
                size++;
            }


        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s">The dependent to be removed, t depends on s</param>
        /// <param name="t">The dependee to be removed, t cannot be evaluated before s</param>
        public void RemoveDependency(string s, string t)
        {
            // if the dependents does not contain 's', we just return because that means there is no pair to remove
            if (!(dependents.ContainsKey(s))) { return; }

            if (!dependents[s].Contains(t)) { return; }
            // if there is only one dependee at 's', we just remove it 
            if (dependents[s].Count == 1)
            {

                dependents[s].Remove(t);

            }
            // if the dependents contains more than one item,
            // we check to see if the dependee is in the pair, if it is we delete it
            else
            {
                dependents[s].Remove(t);
            }

            // now we are concerned with the deleting the reverse pair in the dependees dictionary
            // if the dependees has only one item in it, we can just remove it
            if (dependees[t].Count == 1)
            {
                dependees.Remove(t);

            }

            // if the dependees contains more than one item, we check to see if the dependees contains the dependent
            // and remove it if it does
            else
            {
                dependees[t].Remove(s);
            }
            // we only decrement the size after we get rid of both the dependent pair and dependee pair
            size--;

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        /// <param name="s"> Who's dependents we want to replace. </param>
        /// <param name="newDependents"> The new dependents that are going to replace 's' old dependents </param>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //this checks to see if we are adding an empty dependee to the old dependent
            if (newDependents.Count() == 0)
            {
                //if we are, we create an empty string IEnumerable object and add it to the dependent
                HashSet<string> empty = new HashSet<string>();
                dependents[s] = empty;
                return;

            }
            // if the dependees contains the old dependent, meaning that their might be one or more pairs that include the dependent
            if (dependents.ContainsKey(s))
            {
                // we look at each old dependents that rely on 's' and remove them 
                foreach (var oldDependent in dependents[s])
                {
                    RemoveDependency(s, oldDependent);

                }

                // we look at each of the new dependents that we will be adding and add them as dependees
                foreach (var newDependent in newDependents)
                {
                    AddDependency(s, newDependent);

                }
           

            }
            // we look at the dependents dictionary and see if it contains 's'
            if (dependees.ContainsKey(s))
            {
                // we get each of old dependees and remove them
                foreach (var oldDependee in dependees[s])
                {
                    RemoveDependency(s, oldDependee);
                    

                }
                // we then look at the new dependents and add those as ordered pairs
                foreach (var newDependee in newDependents)
                {
                    AddDependency(s, newDependee);
                }


            }

            

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        /// <param name="s"> Who's dependees we want to replace   </param>
        /// <param name="newDependees">   The new dependees that are going to replace 's' old dependees  </param>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // checking to see if the new dependees that we will be adding are empty
            if (newDependees.Count() == 0)
            {
                // if it is empty, we create a new empty IEnumerable object and add it 
                HashSet<string> empty = new HashSet<string>();
                dependees[s] = empty;
                return;

            }
            // if the dependees contains our dependee we want to keep
            if (dependees.ContainsKey(s))
            {
                // we look at each of the old dependees and remove them
                foreach (var oldDependee in dependees[s])
                {
                    RemoveDependency(oldDependee, s);

                }

                // we look at each of the new dependees and add them as ordered pairs
                foreach (var newDependee in newDependees)
                {
                    AddDependency(newDependee, s);
                }

            }

            else
            {
                foreach (var newDependee in newDependees)
                {
                    AddDependency(newDependee, s);
                }
            }

        }

    }

}
