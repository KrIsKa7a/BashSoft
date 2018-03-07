using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InexistingStudentException : Exception
    {
        private const string InexistingStudentInDataBase =
            "The user name for the student you are trying to get does not exist!";

        public InexistingStudentException()
            : base(InexistingStudentInDataBase)
        {

        }

        public InexistingStudentException(string message)
            : base(message)
        {

        }
    }
}
