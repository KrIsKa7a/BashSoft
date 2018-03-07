using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.Models
{
    class Student
    {
        private string userName;
        private Dictionary<string, Course> enrolledCourse;
        private Dictionary<string, double> marksByCourseName;

        public Student(string userName)
        {
            this.UserName = userName;
            this.enrolledCourse = new Dictionary<string, Course>();
            this.marksByCourseName = new Dictionary<string, double>();
        }

        public IReadOnlyDictionary<string, Course> EnrolledCourse
        {
            get { return this.enrolledCourse; }
        }

        public IReadOnlyDictionary<string, double> MarksByCourseName
        {
            get { return this.marksByCourseName; }
        }

        public string UserName
        {
            get { return this.userName; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidStringException();
                }

                this.userName = value;
            }
        }

        public void EnrollInCourse(Course course)
        {
            if (this.enrolledCourse.ContainsKey(course.Name))
            {
                throw new DuplicateEntryInStructureException(this.userName, course.Name);
            }

            this.enrolledCourse.Add(course.Name, course);
        }

        public void SetMarksOnCourse(string courseName, params int[] scores)
        {
            if (!this.enrolledCourse.ContainsKey(courseName))
            {
                throw new InvalidOperationException (ExceptionMessages.NotEnrolledInCourse);
            }

            if (scores.Length > Course.NumberOfTasksOnExam)
            {
                throw new InvalidNumberOfScoresException();
            }

            this.marksByCourseName.Add(courseName, CalculateMark(scores));
        }

        private double CalculateMark(int[] scores)
        {
            double percentageOfSolvedExam = scores.Sum() / (double)(Course.NumberOfTasksOnExam * Course.MaxScoreOnExamTask);
            double mark = percentageOfSolvedExam * 4 + 2;

            return mark;
        }
    }
}
