using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    class InvalidStringException : Exception
    {
        private const string NullOrEmpty = "The value of the variable CANNOT be null or empty!";

        public InvalidStringException()
            : base(NullOrEmpty)
        {

        }

        public InvalidStringException(string message)
            : base(message)
        {

        }
    }
}
