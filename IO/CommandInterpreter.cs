using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Contracts.StudentRepositoryContracts;
using BashSoft.Contracts.TesterContracts;
using BashSoft.Exceptions;
using BashSoft.IO.Commands;

namespace BashSoft
{
    public class CommandInterpreter : IInterpreter
    {
        private IContentComparer judge;
        private IDatabase studentRepository;
        private IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer tester, IDatabase studentRepository, IDirectoryManager iOManager)
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

        private IExecutable ParseCommand(string input, string[] data, string command)
        {
            var parametersForConstructors = new object[]
            {
                input, data
            };

            var wantedCommandType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .First(
                    type => type.GetCustomAttributes(typeof(AliasAttribute))
                    .Where(a => a.Equals(command))
                    .ToArray().Length > 0
                );

            var typeOfCommandInterpreter = typeof(CommandInterpreter);

            var commandExe = (Command)Activator.CreateInstance
                (wantedCommandType, parametersForConstructors);

            var fieldsOfCommand = wantedCommandType
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldsOfCommandInterpreter = typeOfCommandInterpreter
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fieldsOfCommand)
            {
                var atr = field.GetCustomAttributes(typeof(InjectAttribute));
                if (atr != null)
                {
                    if (fieldsOfCommandInterpreter.Any(f => f.FieldType == field.FieldType))
                    {
                        field
                            .SetValue(commandExe,
                            fieldsOfCommandInterpreter
                                .First(f => f.FieldType == field.FieldType)
                                .GetValue(this));
                    }
                }
            }

            return commandExe;
        }
    }
}
