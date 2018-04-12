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
    [Alias("dropdb")]
    public class DropDatabaseCommand : Command
    {
        [Inject]
        private IDatabase studentRepository;

        public DropDatabaseCommand(string input, string[] data) 
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 1)
            {
                throw new InvalidCommandException(this.Input);
            }

            this.studentRepository.UnloadData();
            OutputWriter.DisplayException("Database dropped!");
        }
    }
}
