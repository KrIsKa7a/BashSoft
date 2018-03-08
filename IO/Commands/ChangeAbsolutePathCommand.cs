using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public class ChangeAbsolutePathCommand : Command
    {
        public ChangeAbsolutePathCommand(string input, string[] data, Tester judge, StudentRepository studentRepository, IOManager iOManager) : base(input, data, judge, studentRepository, iOManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var absPath = this.Data[1];
                this.InputOutputManager.ChangeCurrentDirectoryAbsolute(absPath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
