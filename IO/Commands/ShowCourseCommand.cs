using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Contracts.StudentRepositoryContracts;
using BashSoft.Contracts.TesterContracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    [Alias("show")]
    public class ShowCourseCommand : Command
    {
        [Inject]
        private IDatabase studentRepository;

        public ShowCourseCommand(string input, string[] data) 
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                string courseName = this.Data[1];
                this.studentRepository.GetAllStudentsFromCourse(courseName);
            }
            else if (this.Data.Length == 3)
            {
                string courseName = this.Data[1];
                string studentname = this.Data[2];
                this.studentRepository.GetStudentScoresFromCourse(studentname, courseName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
