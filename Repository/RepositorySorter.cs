using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft
{
    public class RepositorySorter
    {
        public void OrderAndTake(Dictionary<string, double> wantedData,
            string comparison, int studentsToTake)
        {
            comparison = comparison.ToLower();

            if (comparison == "ascending")
            {
                PrintStudents(wantedData
                    .OrderBy(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(a => a.Key, b => b.Value));
            }
            else if (comparison == "descending")
            {
                PrintStudents(wantedData
                    .OrderByDescending(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(a => a.Key, b => b.Value)); ;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidComparisonQuery);
            }
        }

        private void PrintStudents(Dictionary<string, double> studentsSorted)
        {
            foreach (var kvp in studentsSorted)
            {
                OutputWriter.PrintStudent(kvp);
            }
        }
    }
}
