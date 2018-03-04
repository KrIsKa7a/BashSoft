﻿using System;
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
                    throw new ArgumentException(nameof(this.userName), ExceptionMessages.NullOrEmptyValue);
                }

                this.userName = value;
            }
        }

        public void EnrollInCourse(Course course)
        {
            if (this.enrolledCourse.ContainsKey(this.userName))
            {
                OutputWriter.DisplayException(String.Format(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse, this.userName, course.Name));
                return;
            }

            this.enrolledCourse.Add(course.Name, course);
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
