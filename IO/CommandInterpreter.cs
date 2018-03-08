using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using BashSoft.Exceptions;
using BashSoft.IO.Commands;

namespace BashSoft
{
    public class CommandInterpreter
    {
        private Tester judge;
        private StudentRepository studentRepository;
        private IOManager inputOutputManager;

        public CommandInterpreter(Tester tester, StudentRepository studentRepository, IOManager iOManager)
        {
            this.judge = tester;
            this.studentRepository = studentRepository;
            this.inputOutputManager = iOManager;
        }

        public void InterpredCommand(string input)
        {
            string[] data = input.Split(' ');
            string commandName = data[0];

            try
            {
                var commmand = ParseCommand(input, data, commandName);
                commmand.Execute();
            }
            catch(Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }
        }

        private Command ParseCommand(string input, string[] data, string command)
        {
            switch (command)
            {
                case "open":
                    return new OpenFileCommand(input, data, judge, studentRepository, inputOutputManager);
                case "mkdir":
                    return new MakeDirectoryCommand(input, data, judge, studentRepository, inputOutputManager);
                case "ls":
                    return new TraverseFoldersCommand(input, data, judge, studentRepository, inputOutputManager);
                case "cmp":
                    return new CompareFilesCommand(input, data, judge, studentRepository, inputOutputManager);
                case "cdRel":
                    return new ChangeRelativePathCommand(input, data, judge, studentRepository, inputOutputManager);
                case "cdAbs":
                    return new ChangeAbsolutePathCommand(input, data, judge, studentRepository, inputOutputManager);
                case "readDb":
                    return new ReadDatabaseCommand(input, data, judge, studentRepository, inputOutputManager);
                case "dropdb":
                    return new DropDatabaseCommand(input, data, judge, studentRepository, inputOutputManager);
                case "help":
                    return new GetHelpCommand(input, data, judge, studentRepository, inputOutputManager);
                case "filter":
                    return new PrintFilteredStudentsCommand(input, data, judge, studentRepository, inputOutputManager);
                case "order":
                    return new PrintOrderedStudentsCommand(input, data, judge, studentRepository, inputOutputManager);
                //TODO : Implement
                //case "decOrder":
                //    //TODO
                //    break;
                //case "download":
                //    //TODO
                //    break;
                //case "downloadAsynch":
                //    //TODO
                //    break;
                case "show":
                    return new ShowCourseCommand(input, data, judge, studentRepository, inputOutputManager);
                default:
                    throw new InvalidCommandException(input);
            }
        }
    }
}
