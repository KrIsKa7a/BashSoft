using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BashSoft
{
    public static class StudentRepository
    {
        public static bool isDataInitialized = false;
        private static Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;

        public static void InitializeData(string fileName)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Read data...");
                studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
                ReadData(fileName);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.DataAlreadyInitialisedException);
            }
        }

        private static void ReadData(string fileName)
        {
            string path = SessionData.currentPath + "\\" + fileName;

            if (File.Exists(path))
            {
                var pattern = @"^([A-Z]+[a-z#+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Z][a-z]{0,3}\d{2}_\d{2,4})\s+(\d+)$";
                Regex regex = new Regex(pattern);

                string[] allInputLines = File.ReadAllLines(path);

                for (int line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]) && regex.IsMatch(allInputLines[line]))
                    {
                        var currentMatch = regex.Match(allInputLines[line]);
                        //var inputArgs = allInputLines[line].Split(' ');
                        string course = currentMatch.Groups[1].Value;
                        var courseYear = int.Parse(course.Substring(course.LastIndexOf('_') + 1));

                        if (courseYear < 2014 || courseYear > DateTime.Now.Year)
                        {
                            continue;
                        }

                        var student = currentMatch.Groups[2].Value;
                        int mark;
                        bool hasParsed = int.TryParse(currentMatch.Groups[3].Value, out mark);

                        if (hasParsed)
                        {
                            if (!studentsByCourse.ContainsKey(course))
                            {
                                studentsByCourse[course] = new Dictionary<string, List<int>>();
                            }

                            if (!studentsByCourse[course].ContainsKey(student))
                            {
                                studentsByCourse[course][student] = new List<int>();
                            }

                            studentsByCourse[course][student].Add(mark);

                            isDataInitialized = true;
                        }
                    }
                }

                OutputWriter.WriteMessageOnNewLine("Data read!");
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
            }
        }

        private static bool IsQueryForCoursePossible(string course)
        {
            if (isDataInitialized)
            {
                if (studentsByCourse.ContainsKey(course))
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

        private static bool IsQueryForStudentPossible(string course, string student)
        {
            if (IsQueryForCoursePossible(course) && studentsByCourse[course].ContainsKey(student))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            }

            return false;
        }

        public static void GetStudentScoresFromCourse(string student, string course)
        {
            if (IsQueryForStudentPossible(course, student))
            {
                OutputWriter.PrintStudent(new KeyValuePair<string, List<int>>(student, studentsByCourse[course][student]));
            }
        }

        public static void GetAllStudentsFromCourse(string course)
        {
            if (IsQueryForCoursePossible(course))
            {
                OutputWriter.WriteMessageOnNewLine($"{course}:");

                foreach (var studentMarksEntry in studentsByCourse[course])
                {
                    OutputWriter.PrintStudent(studentMarksEntry);
                }
            }
        }

        public static void FilterAndTake(string courseName, string givenFilter,
            int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[courseName].Count;
                }

                RepositoryFilters.FilterAndTake(studentsByCourse[courseName],
                    givenFilter, studentsToTake.Value);
            }
        }

        public static void OrderAndTake(string courseName, string comparison,
            int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[courseName].Count;
                }

                RepositorySorters.OrderAndTake(studentsByCourse[courseName],
                    comparison, studentsToTake.Value);
            }
        }
    }
}
