using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Models
{
    class Course
    {
        public const int NumberOfTasksOnExam = 5;
        public const int MaxScoreOnExamTask = 100;

        public string name;
        public Dictionary<string, Student> studentsByName;

        public Course(string name)
        {
            this.name = name;
            this.studentsByName = new Dictionary<string, Student>();
        }

        public void EnrollStudent(Student student)
        {
            if (this.studentsByName.ContainsKey(student.userName))
            {
                OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse, student.userName, this.name));

                return;
            }

            this.studentsByName.Add(student.userName, student);
        }
    }
}
