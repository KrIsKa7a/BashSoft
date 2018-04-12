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
    [Alias("cmp")]
    public class CompareFilesCommand : Command
    {
        [Inject]
        private IContentComparer judge;

        public CompareFilesCommand(string input, string[] data) 
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 3)
            {
                var firstFilePath = this.Data[1];
                var secondFilePath = this.Data[2];
                this.judge.CompareContents(firstFilePath, secondFilePath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
