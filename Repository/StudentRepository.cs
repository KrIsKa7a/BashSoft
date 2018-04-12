using BashSoft.Contracts.ModelsContracts;
using BashSoft.Contracts.SortedListContracts;
using BashSoft.Contracts.StudentRepositoryContracts;
using BashSoft.DataStructures;
using BashSoft.Exceptions;
using BashSoft.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BashSoft
{
    public class StudentRepository : IDatabase
    {
        private bool isDataInitialized;
        private IDataFilter filter;
        private IDataSorter sorter;
        private Dictionary<string, ICourse> courses;
        private Dictionary<string, IStudent> students;

        public StudentRepository(IDataSorter sorter, IDataFilter filter)
        {
            this.filter = filter;
            this.sorter = sorter;
        }

        public void LoadData(string fileName)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Read data...");
                this.students = new Dictionary<string, IStudent>();
                this.courses = new Dictionary<string, ICourse>();
                ReadData(fileName);
            }
            else
            {
                throw new DataAlreadyInitialisedException();
            }
        }

        public void UnloadData()
        {
            if (!isDataInitialized)
            {
                throw new DataNotInitializedException();
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
                                throw new InvalidScoreException();
                            }

                            if (scores.Length > SoftUniCourse.NumberOfTasksOnExam)
                            {
                                throw new InvalidNumberOfScoresException();
                            }

                            if (!this.students.ContainsKey(student))
                            {
                                this.students[student] = new SoftUniStudent(student);
                            }

                            if (!this.courses.ContainsKey(course))
                            {
                                this.courses[course] = new SoftUniCourse(course);
                            }

                            ICourse currentCourse = this.courses[course];
                            IStudent currentStudent = this.students[student];

                            currentStudent.EnrollInCourse(currentCourse);
                            currentStudent.SetMarksOnCourse(course, scores);

                            currentCourse.EnrollStudent(currentStudent);
                        }
                        catch(InvalidNumberOfScoresException inose)
                        {
                            OutputWriter.DisplayException(inose.Message);
                        }
                        catch(InvalidScoreException ise)
                        {
                            OutputWriter.DisplayException(ise.Message);
                        }
                        catch (FormatException fex)
                        {
                            OutputWriter.DisplayException(fex.Message + $".at line : {line}");
                        }
                    }
                }

                this.isDataInitialized = true;
                OutputWriter.WriteMessageOnNewLine("Data read!");
            }
            else
            {
                throw new InvalidPathException();
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
                    throw new CourseNotFoundException();
                }
            }
            else
            {
                throw new DataNotInitializedException();
            }
        }

        private bool IsQueryForStudentPossible(string course, string student)
        {
            if (IsQueryForCoursePossible(course) && this.courses[course].StudentsByName.ContainsKey(student))
            {
                return true;
            }
            else
            {
                throw new InexistingStudentException();
            }
        }

        public void GetStudentScoresFromCourse(string student, string course)
        {
            if (IsQueryForStudentPossible(course, student))
            {
                OutputWriter.PrintStudent(new KeyValuePair<string, double>(student, this.courses[course].StudentsByName[student].MarksByCourseName[course]));
            }
        }

        public void GetAllStudentsFromCourse(string course)
        {
            if (IsQueryForCoursePossible(course))
            {
                OutputWriter.WriteMessageOnNewLine($"{course}:");

                foreach (var studentMarksEntry in this.courses[course].StudentsByName)
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
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }

                Dictionary<string, double> marks =
                     this.courses[courseName].StudentsByName.ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);

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
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }

                Dictionary<string, double> marks =
                    this.courses[courseName].StudentsByName.ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);

                this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);
            }
        }

        public ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> cmp)
        {
            SimpleSortedList<ICourse> sortedCourses = new SimpleSortedList<ICourse>(cmp);
            sortedCourses.AddAll(this.courses.Values);

            return sortedCourses;
        }

        public ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> cmp)
        {
            SimpleSortedList<IStudent> sortedStudents = new SimpleSortedList<IStudent>(cmp);
            sortedStudents.AddAll(this.students.Values);

            return sortedStudents;
        }
    }
}
