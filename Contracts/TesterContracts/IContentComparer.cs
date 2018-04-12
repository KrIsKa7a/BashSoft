using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts.TesterContracts
{
    public interface IContentComparer
    {
        void CompareContents(string userOutputPath, string expectedOutputPath);
    }
}
