using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts
{
    public interface IDirectoryCreator
    {
        void CreateDirectoryInCurrentFolder(string name);
    }
}
