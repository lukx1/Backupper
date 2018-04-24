using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Objects.AdminExceptions
{
    public class AdminSecurityException : AdminException
    {
        public AdminSecurityException(string message) : base (message) { }
    }
}