﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class SessionMessage : INetMessage
    {
        public Guid sessionUuid;
    }
}
