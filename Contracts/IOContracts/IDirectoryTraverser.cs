using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts
{
    public interface IDirectoryTraverser
    {
        void TraverseDirectory(int depth);
    }
}
