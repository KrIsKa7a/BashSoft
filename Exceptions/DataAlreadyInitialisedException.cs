using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class DataAlreadyInitialisedException : Exception
    {
        private const string AlreadyInitializedData = "Data is already initialized!";

        public DataAlreadyInitialisedException()
            : base(AlreadyInitializedData)
        {

        }

        public DataAlreadyInitialisedException(string message)
            : base(message)
        {

        }
    }
}
