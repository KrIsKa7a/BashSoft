using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidFilterException : Exception
    {
        private const string InvalidStudentFilter =
            "The given filter is not one of the following: excellent/average/poor";

        public InvalidFilterException()
            : base(InvalidStudentFilter)
        {

        }

        public InvalidFilterException(string message)
            : base(message)
        {

        }
    }
}
