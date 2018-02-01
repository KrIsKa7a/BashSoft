using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                string[] allInputLines = File.ReadAllLines(path);

                for (int line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]))
                    {
                        var inputArgs = allInputLines[line].Split(' ');
                        string course = inputArgs[0];
                        var student = inputArgs[1];
                        var mark = int.Parse(inputArgs[2]);

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
                        OutputWriter.WriteMessageOnNewLine("Data read!");
                    }
                }
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
    }
}
