using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Exceptions
{
    public class NonExistingUserException : Exception
    {
        public NonExistingUserException() : base()
        { }

        public NonExistingUserException(string message) : base(message)
        { }
    }
}