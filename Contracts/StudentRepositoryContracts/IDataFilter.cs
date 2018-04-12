using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts.StudentRepositoryContracts
{
    public interface IDataFilter
    {
        void FilterAndTake
            (Dictionary<string, double> studentsWithMarks,
            string wantedFilter, int studentsToTake);
    }
}
