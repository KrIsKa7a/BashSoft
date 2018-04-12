using BashSoft.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft
{
    public class InputReader : IReader
    {
        private const string endCommand = "quit";

        private IInterpreter interpreter;

        public InputReader(IInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public void StartReadingCommands()
        {
            OutputWriter.WriteMessage($"{SessionData.currentPath}> ");
            string input = Console.ReadLine().Trim();

            while (input != endCommand)
            {
                interpreter.InterpredCommand(input);
                OutputWriter.WriteMessage($"{SessionData.currentPath}> ");
                input = Console.ReadLine();

                input = input.Trim();
            }
        }
    }
}
