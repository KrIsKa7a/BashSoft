using System;

namespace BashSoft
{
    class Launcher
    {
        static void Main(string[] args)
        {
            IOManager.ChangeCurrentDirectoryAbsolute(@"D:\Homeworks");
            IOManager.TraverseDirectory(3);
        }
    }
}
