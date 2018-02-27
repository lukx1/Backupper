using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    public enum Permission
    {
        DEBUG = -1,
        SKIP = 1,
        MANAGEUSERS = 2,
        MANAGEDAEMONS = 3,
        MANAGEPRESHAREDKEYS = 4,
        MANAGETASKS = 5,
        MANAGEOTHERS = 6,
    }
}