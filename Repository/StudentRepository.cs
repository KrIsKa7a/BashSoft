using BashSoft.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BashSoft
{
    public class StudentRepository
    {
        private bool isDataInitialized;
        //private Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;
        private RepositoryFilter filter;
        private RepositorySorter sorter;
        private Dictionary<string, Course> courses;
        private Dictionary<string, Student> students;

        public StudentRepository(RepositorySorter sorter, RepositoryFilter filter)
        {
            this.filter = filter;
            this.sorter = sorter;
            //this.studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
        }

        public void LoadData(string fileName)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Read data...");
                this.students = new Dictionary<string, Student>();
                this.courses = new Dictionary<string, Course>();
                ReadData(fileName);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.DataAlreadyInitialisedException);
            }
        }

        public void UnloadData()
        {
            if (!isDataInitialized)
            {
                OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }

            this.students = null;
            this.courses = null;
            this.isDataInitialized = false;
        }

        private void ReadData(string fileName)
        {
            string path = SessionData.currentPath + "\\" + fileName;

            if (File.Exists(path))
            {
                var pattern = @"^([A-Z][a-zA-Z#\+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)$";
                Regex regex = new Regex(pattern);

                string[] allInputLines = File.ReadAllLines(path);

                for (int line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]) && regex.IsMatch(allInputLines[line]))
                    {
                        var currentMatch = regex.Match(allInputLines[line]);
                        string course = currentMatch.Groups[1].Value;
                        var courseYear = int.Parse(course.Substring(course.LastIndexOf('_') + 1));

                        if (courseYear < 2014 || courseYear > DateTime.Now.Year)
                        {
                            continue;
                        }

                        var student = currentMatch.Groups[2].Value;
                        var scoresStr = currentMatch.Groups[3].Value;

                        try
                        {
                            int[] scores = scoresStr
                                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

                            if (scores.Any(s => s < 0 || s > 100))
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidScore);
                            }

                            if (scores.Length > Course.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                            }

                            if (!this.students.ContainsKey(student))
                            {
                                this.students[student] = new Student(student);
                            }

                            if (!this.courses.ContainsKey(course))
                            {
                                this.courses[course] = new Course(course);
                            }

                            Course currentCourse = this.courses[course];
                            Student currentStudent = this.students[student];

                            currentStudent.EnrollInCourse(currentCourse);
                            currentStudent.SetMarksOnCourse(course, scores);

                            currentCourse.EnrollStudent(currentStudent);
                        }
                        catch (FormatException fex)
                        {
                            Console.WriteLine(fex.Message + $".at line : {line}");
                        }
                        //int mark;
                        //bool hasParsed = int.TryParse(currentMatch.Groups[3].Value, out mark);

                        //if (hasParsed)
                        //{
                        //    if (!studentsByCourse.ContainsKey(course))
                        //    {
                        //        studentsByCourse[course] = new Dictionary<string, List<int>>();
                        //    }

                        //    if (!studentsByCourse[course].ContainsKey(student))
                        //    {
                        //        studentsByCourse[course][student] = new List<int>();
                        //    }

                        //    studentsByCourse[course][student].Add(mark);

                        //    isDataInitialized = true;
                        //}
                    }
                }

                OutputWriter.WriteMessageOnNewLine("Data read!");
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
            }
        }

        private bool IsQueryForCoursePossible(string course)
        {
            if (isDataInitialized)
            {
                if (this.courses.ContainsKey(course))
                {
                    return true;
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }

            return false;
        }

        private bool IsQueryForStudentPossible(string course, string student)
        {
            if (IsQueryForCoursePossible(course) && this.courses[course].studentsByName.ContainsKey(student))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            }

            return false;
        }

        public void GetStudentScoresFromCourse(string student, string course)
        {
            if (IsQueryForStudentPossible(course, student))
            {
                OutputWriter.PrintStudent(new KeyValuePair<string, double>(student, this.courses[course].studentsByName[student].marksByCourseName[course]));
            }
        }

        public void GetAllStudentsFromCourse(string course)
        {
            if (IsQueryForCoursePossible(course))
            {
                OutputWriter.WriteMessageOnNewLine($"{course}:");

                foreach (var studentMarksEntry in this.courses[course].studentsByName)
                {
                    this.GetStudentScoresFromCourse(course, studentMarksEntry.Key);
                }
            }
        }

        public void FilterAndTake(string courseName, string givenFilter,
            int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].studentsByName.Count;
                }

                Dictionary<string, double> marks =
                     this.courses[courseName].studentsByName.ToDictionary(x => x.Key, x => x.Value.marksByCourseName[courseName]);

                this.filter.FilterAndTake(marks, givenFilter, studentsToTake.Value);
            }
        }

        public void OrderAndTake(string courseName, string comparison,
            int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].studentsByName.Count;
                }

                Dictionary<string, double> marks =
                    this.courses[courseName].studentsByName.ToDictionary(x => x.Key, x => x.Value.marksByCourseName[courseName]);

                this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);
            }
        }
    }
}
