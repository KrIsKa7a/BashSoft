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
    [Alias("readDb")]
    public class ReadDatabaseCommand : Command
    {
        [Inject]
        private IDatabase studentRepository;

        public ReadDatabaseCommand(string input, string[] data) 
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var fileName = this.Data[1];
                this.studentRepository.LoadData(fileName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
