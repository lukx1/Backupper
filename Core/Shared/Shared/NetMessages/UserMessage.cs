﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class UserMessage : SessionMessage
    {
        public List<DbUser> Users;
    }
}
