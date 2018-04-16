using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    /// <summary>
    /// SQL PermissionEnum
    /// </summary>
    public enum Permission
    {
        SKIP = 1,
        LOGIN = 2,
        MANAGEPRESHARED = 3,
        MANAGESELFUSER = 4,
        MANAGEOTHERUSERS = 5,
        MANAGESELFDAEMONS = 6,
        MANAGEOTHERDAEMONS = 7,
        MANAGEPERMISSION = 8,
        MANAGEGROUPS = 9,
        DAEMONFETCHTASKS = 10,
        MANAGETIMES = 11,
        MANAGELOCATIONS = 12,
        MANAGECREDENTIALS = 13,
        MANAGESERVERSTATUS = 14,
        MANAGELOGS = 15,
        MANAGEPOWER = 16,
        MANAGEEMAIL = 17
    }
}