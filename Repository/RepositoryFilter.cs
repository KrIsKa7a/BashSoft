using BashSoft.Contracts.StudentRepositoryContracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft
{
    public class RepositoryFilter : IDataFilter
    {
        public void FilterAndTake(Dictionary<string, double> studentsWithMarks,
            string wantedFilter, int studentsToTake)
        {
            if (wantedFilter == "excellent")
            {
                FilterAndTake(studentsWithMarks, x => x >= 5.0, studentsToTake);
            }
            else if (wantedFilter == "average")
            {
                FilterAndTake(studentsWithMarks, x => x >= 3.50 && x < 5.00, studentsToTake);
            }
            else if (wantedFilter == "poor")
            {
                FilterAndTake(studentsWithMarks, x => x < 3.50, studentsToTake);
            }
            else
            {
                throw new InvalidFilterException();
            }
        }

        private void FilterAndTake(Dictionary<string, double> studentsWithMarks,
            Predicate<double> givenFilter, int studentsToTake)
        {
            var counterForPrinted = 0;

            foreach (var studentMark in studentsWithMarks)
            {
                if (counterForPrinted == studentsToTake)
                {
                    break;
                }

                if (givenFilter(studentMark.Value))
                {
                    OutputWriter.PrintStudent(studentMark);
                    counterForPrinted++;
                }
            }
        }
    }
}
