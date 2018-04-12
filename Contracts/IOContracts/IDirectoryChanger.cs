using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts
{
    public interface IDirectoryChanger
    {
        void ChangeCurrentDirectoryRelative(string relPath);

        void ChangeCurrentDirectoryAbsolute(string absPath);
    }
}
