using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.Models
{
    class Student
    {
        public string userName;
        public Dictionary<string, Course> enrolledCourse;
        public Dictionary<string, double> marksByCourseName;

        public Student(string userName)
        {
            this.userName = userName;
            this.enrolledCourse = new Dictionary<string, Course>();
            this.marksByCourseName = new Dictionary<string, double>();
        }

        public void EnrollInCourse(Course course)
        {
            if (this.enrolledCourse.ContainsKey(this.userName))
            {
                OutputWriter.DisplayException(String.Format(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse, this.userName, course.name));
                return;
            }

            this.enrolledCourse.Add(course.name, course);
        }

        public void SetMarksOnCourse(string courseName, params int[] scores)
        {
            if (!this.enrolledCourse.ContainsKey(courseName))
            {
                OutputWriter.DisplayException(ExceptionMessages.NotEnrolledInCourse);
                return;
            }

            if (scores.Length > Course.NumberOfTasksOnExam)
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                return;
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
