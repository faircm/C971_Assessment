using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Resources
{
    // Custom exception to be thrown when an illegal datetime is provided by the user (i.e., start date occurs after end date, due date occurs before start date, etc.)
    internal class IllegalDateTimeException : Exception
    {
        public IllegalDateTimeException()
        {
        }

        public IllegalDateTimeException(string message) : base(message)
        {
        }
    }
}