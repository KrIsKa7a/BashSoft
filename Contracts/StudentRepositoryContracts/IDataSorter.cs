using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts.StudentRepositoryContracts
{
    public interface IDataSorter
    {
        void OrderAndTake
            (Dictionary<string, double> wantedData, string comparison, int studentsToTake);
    }
}
