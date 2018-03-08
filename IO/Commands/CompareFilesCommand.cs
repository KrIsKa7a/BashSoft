using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public class CompareFilesCommand : Command
    {
        public CompareFilesCommand(string input, string[] data, Tester judge, StudentRepository studentRepository, IOManager iOManager) : base(input, data, judge, studentRepository, iOManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 3)
            {
                var firstFilePath = this.Data[1];
                var secondFilePath = this.Data[2];
                this.Judge.CompareContents(firstFilePath, secondFilePath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
