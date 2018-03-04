using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Exceptions
{
    public class NotMatchingPasswordException : Exception
    {
        public NotMatchingPasswordException() : base()
        { }

        public NotMatchingPasswordException(string message) : base(message)
        { }
    }
}