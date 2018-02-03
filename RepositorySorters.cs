using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft
{
    public static class RepositorySorters
    {
        public static void OrderAndTake(Dictionary<string, List<int>> wantedData,
            string comparison, int studentsToTake)
        {
            comparison = comparison.ToLower();

            if (comparison == "ascending")
            {
                OrderAndTake(wantedData, studentsToTake, CompareInOrder);
            }
            else if (comparison == "descending")
            {
                OrderAndTake(wantedData, studentsToTake, CompareInDescendingOrder);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidComparisonQuery);
            }
        }

        private static void OrderAndTake(Dictionary<string, List<int>> wantedData,
            int takeCount,
            Func<KeyValuePair<string, List<int>>, KeyValuePair<string, List<int>>, int> comparisonFunc)
        {
            var dict = GetSortedStudents(wantedData, takeCount, comparisonFunc);

            foreach (var kvp in dict)
            {
                OutputWriter.PrintStudent(kvp);
            }
        }

        private static Dictionary<string, List<int>> GetSortedStudents(
            Dictionary<string, List<int>> studentsWanted, int takeCount,
            Func<KeyValuePair<string, List<int>>, KeyValuePair<string, List<int>>, int> comparison)
        {
            int valuesTaken = 0;

            var studentsSorted = new Dictionary<string, List<int>>();
            var nextInOrder = new KeyValuePair<string, List<int>>();
            bool isSorted = false;

            while (valuesTaken < takeCount)
            {
                isSorted = true;

                foreach (var studentsWithScore in studentsWanted)
                {
                    if (!String.IsNullOrEmpty(nextInOrder.Key))
                    {
                        var comparisonResult = comparison(studentsWithScore, nextInOrder);

                        if (comparisonResult >= 0 && !studentsSorted.ContainsKey(studentsWithScore.Key))
                        {
                            nextInOrder = studentsWithScore;
                            isSorted = false;
                        }
                    }
                    else
                    {
                        if (!studentsSorted.ContainsKey(studentsWithScore.Key))
                        {
                            nextInOrder = studentsWithScore;
                            isSorted = false;
                        }
                    }
                }

                if (!isSorted)
                {
                    studentsSorted.Add(nextInOrder.Key, nextInOrder.Value);
                    valuesTaken++;
                    nextInOrder = new KeyValuePair<string, List<int>>();
                }
            }

            return studentsSorted;
        }

        private static int CompareInOrder(KeyValuePair<string, List<int>> firstValue,
            KeyValuePair<string, List<int>> secondValue)
        {
            var totalOfFirstMarks = 0;
            foreach (var mark in firstValue.Value)
            {
                totalOfFirstMarks += mark;
            }

            var totalOfSecondMarks = 0;
            foreach (var mark in secondValue.Value)
            {
                totalOfSecondMarks += mark;
            }

            return totalOfSecondMarks.CompareTo(totalOfFirstMarks);
        }

        private static int CompareInDescendingOrder(KeyValuePair<string, List<int>> firstValue,
            KeyValuePair<string, List<int>> secondValue)
        {
            var totalOfFirstMarks = 0;
            foreach (var mark in firstValue.Value)
            {
                totalOfFirstMarks += mark;
            }

            var totalOfSecondMarks = 0;
            foreach (var mark in secondValue.Value)
            {
                totalOfSecondMarks += mark;
            }

            return totalOfFirstMarks.CompareTo(totalOfSecondMarks);
        }
    }
}
