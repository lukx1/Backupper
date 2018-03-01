using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    /// <summary>
    /// SQL GroupEnum view
    /// </summary>
    public enum Group
    {
        SERVER = -999,
        DEBUGGROUP = -1,
        ADMINS = 1,
        DAEMONS = 2,
    }
}