using BashSoft.Models;
using System;
using System.Collections.Generic;

namespace BashSoft.Contracts.ModelsContracts
{
    public interface ICourse : IComparable<ICourse>
    {
        string Name { get; }
        IReadOnlyDictionary<string, IStudent> StudentsByName { get; }

        void EnrollStudent(IStudent student);
    }
}