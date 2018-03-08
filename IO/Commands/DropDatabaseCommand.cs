using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public class DropDatabaseCommand : Command
    {
        public DropDatabaseCommand(string input, string[] data, Tester judge, StudentRepository studentRepository, IOManager iOManager) : base(input, data, judge, studentRepository, iOManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 1)
            {
                throw new InvalidCommandException(this.Input);
            }

            this.StudentRepository.UnloadData();
            OutputWriter.DisplayException("Database dropped!");
        }
    }
}
