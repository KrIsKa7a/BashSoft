using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts
{
    public interface IInterpreter
    {
        void InterpredCommand(string command);
    }
}
