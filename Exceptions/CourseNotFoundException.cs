using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        private const string NotFoundMessage = "The course you are trying to get does not exist in the data base!";

        public CourseNotFoundException()
            : base(NotFoundMessage)
        {

        }

        public CourseNotFoundException(string message)
            : base(message)
        {

        }
    }
}
