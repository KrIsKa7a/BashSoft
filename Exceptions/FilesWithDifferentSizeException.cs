using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class FilesWithDifferentSizeException : Exception
    {
        private const string ComparisonOfFilesWithDifferentSizes =
            "Files not of equal size, certain mismatch.";

        public FilesWithDifferentSizeException()
            : base(ComparisonOfFilesWithDifferentSizes)
        {

        }

        public FilesWithDifferentSizeException(string message)
            : base(message)
        {

        }
    }
}
