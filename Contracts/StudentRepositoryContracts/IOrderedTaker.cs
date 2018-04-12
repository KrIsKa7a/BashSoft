using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts.StudentRepositoryContracts
{
    public interface IOrderedTaker
    {
        void OrderAndTake
            (string courseName, string comparison, int? studentsToTake = null);
    }
}
