using BashSoft.Contracts;
using BashSoft.Contracts.StudentRepositoryContracts;
using BashSoft.Contracts.TesterContracts;
using System;

namespace BashSoft
{
    class Launcher
    {
        static void Main(string[] args)
        {
            IContentComparer tester = new Tester();
            IDirectoryManager ioManager = new IOManager();
            IDatabase studentRepository = new StudentRepository(new RepositorySorter(), new RepositoryFilter());

            IInterpreter commandInterpreter = new CommandInterpreter(tester, studentRepository, ioManager);
            IReader inputReader = new InputReader(commandInterpreter);

            inputReader.StartReadingCommands();
        }
    }
}
