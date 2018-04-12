using BashSoft.Models;
using System;
using System.Collections.Generic;

namespace BashSoft.Contracts.ModelsContracts
{
    public interface IStudent : IComparable<IStudent>
    {
        IReadOnlyDictionary<string, ICourse> EnrolledCourse { get; }
        IReadOnlyDictionary<string, double> MarksByCourseName { get; }
        string UserName { get; }

        void EnrollInCourse(ICourse course);
        void SetMarksOnCourse(string courseName, params int[] scores);
    }
}