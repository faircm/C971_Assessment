using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Resources
{
    // Custom exception to be thrown when an invalid phone number is provided by the user.
    public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException()
        {
        }
    }
}