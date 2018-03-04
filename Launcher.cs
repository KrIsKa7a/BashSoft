using System;

namespace BashSoft
{
    class Launcher
    {
        static void Main(string[] args)
        {
            Tester tester = new Tester();
            IOManager ioManager = new IOManager();
            StudentRepository studentRepository = new StudentRepository(new RepositorySorter(), new RepositoryFilter());

            CommandInterpreter commandInterpreter = new CommandInterpreter(tester, studentRepository, ioManager);
            InputReader inputReader = new InputReader(commandInterpreter);

            inputReader.StartReadingCommands();
        }
    }
}
