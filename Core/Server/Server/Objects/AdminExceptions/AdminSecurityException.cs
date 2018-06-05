using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Objects.AdminExceptions
{
    /// <summary>
    /// Používané např. pro chybné přihlášení a nebo nedostatečné permise
    /// </summary>
    public class AdminSecurityException : AdminException
    {
        public AdminSecurityException(string message) : base (message) { }
    }
}