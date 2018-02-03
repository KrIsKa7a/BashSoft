using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft
{
    public static class RepositoryFilters
    {
        public static void FilterAndTake(Dictionary<string, List<int>> wantedData,
            string wantedFilter, int studentsToTake)
        {
            if (wantedFilter == "excellent")
            {
                FilterAndTake(wantedData, ExcellentFilter, studentsToTake);
            }
            else if (wantedFilter == "average")
            {
                FilterAndTake(wantedData, AverageFilter, studentsToTake);
            }
            else if (wantedFilter == "poor")
            {
                FilterAndTake(wantedData, PoorFilter, studentsToTake);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidStudentFilter);
            }
        }

        private static void FilterAndTake(Dictionary<string, List<int>> wantedData,
            Predicate<double> givenFilter, int studentsToTake)
        {
            var counterForPrinted = 0;

            foreach (var upp in wantedData)
            {
                if (counterForPrinted == studentsToTake)
                {
                    break;
                }

                double averageMark = Average(upp.Value);

                if (givenFilter(averageMark))
                {
                    OutputWriter.PrintStudent(upp);
                    counterForPrinted++;
                }
            }
        }

        private static bool ExcellentFilter(double mark)
        {
            return mark >= 5.00;
        }

        private static bool AverageFilter(double mark)
        {
            return mark < 5.00 && mark >= 3.50;
        }

        private static bool PoorFilter(double mark)
        {
            return mark < 3.50;
        }

        private static double Average(List<int> scoresOnTasks)
        {
            int totalScore = 0;

            foreach (var score in scoresOnTasks)
            {
                totalScore += score;
            }

            var percentageOfAll = totalScore / (scoresOnTasks.Count * 100.0);
            var mark = percentageOfAll * 4 + 2;

            return mark;
        }
    }
}
