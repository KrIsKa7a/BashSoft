using BashSoft.Contracts.ModelsContracts;
using BashSoft.Contracts.SortedListContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts.StudentRepositoryContracts
{
    public interface IRequester
    {
        void GetStudentScoresFromCourse(string student, string course);

        void GetAllStudentsFromCourse(string course);

        ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> cmp);

        ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> cmp);
    }
}
