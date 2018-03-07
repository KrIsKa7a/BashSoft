using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidScoreException : Exception
    {
        private const string InvalidScore = "Score should be between 0 and 100";

        public InvalidScoreException()
            : base(InvalidScore)
        {

        }

        public InvalidScoreException(string message)
            : base(message)
        {

        }
    }
}
