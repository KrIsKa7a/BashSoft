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
    [Alias("order")]
    public class PrintOrderedStudentsCommand : Command
    {
        [Inject]
        private IDatabase studentRepository;

        public PrintOrderedStudentsCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 5)
            {
                string courseName = this.Data[1];
                string comparison = this.Data[2].ToLower();
                string takeCommand = this.Data[3].ToLower();
                string takeQuantity = this.Data[4].ToLower();

                TryParseParametersForOrderAndTake(takeCommand, takeQuantity,
                    courseName, comparison);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }

        private void TryParseParametersForOrderAndTake(string takeCommand, string takeQuantity, string courseName, string comparison)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity == "all")
                {
                    this.studentRepository.OrderAndTake(courseName, comparison);
                }
                else
                {
                    int studentsToTake;
                    bool hasParsed = int.TryParse(takeQuantity, out studentsToTake);

                    if (hasParsed)
                    {
                        this.studentRepository.OrderAndTake(courseName, comparison, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeCommand);
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeCommand);
            }
        }
    }
}
