﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Objects.AdminExceptions
{
    public class AdminException : Exception
    {
        public AdminException(string message) : base(message) { }
    }
}