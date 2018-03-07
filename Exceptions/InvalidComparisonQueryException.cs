using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidComparisonQueryException : Exception
    {
        private const string InvalidComparisonQuery =
            "The comparison query you want, does not exist in the context of the current program!";

        public InvalidComparisonQueryException()
            : base(InvalidComparisonQuery)
        {

        }

        public InvalidComparisonQueryException(string message)
            : base(message)
        {

        }
    }
}
