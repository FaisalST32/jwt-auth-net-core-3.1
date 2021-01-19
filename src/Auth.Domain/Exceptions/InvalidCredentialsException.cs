using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message)
        {

        }
    }
}
