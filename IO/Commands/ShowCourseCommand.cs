using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public class ShowCourseCommand : Command
    {
        public ShowCourseCommand(string input, string[] data, Tester judge, StudentRepository studentRepository, IOManager iOManager) : base(input, data, judge, studentRepository, iOManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                string courseName = this.Data[1];
                this.StudentRepository.GetAllStudentsFromCourse(courseName);
            }
            else if (this.Data.Length == 3)
            {
                string courseName = this.Data[1];
                string studentname = this.Data[2];
                this.StudentRepository.GetStudentScoresFromCourse(studentname, courseName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
