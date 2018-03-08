using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidCommandException : Exception
    {
        private const string message = "The command '{0}' is invalid";

        public InvalidCommandException(string input)
            : base(String.Format(message, input))
        {
            
        }
    }
}
