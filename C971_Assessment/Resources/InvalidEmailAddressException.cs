using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Models
{
    // Custom exception to be thrown when an invalid email is provided by the user.
    internal class InvalidEmailAddressException : Exception
    {
        public InvalidEmailAddressException()
        {
        }
    }
}