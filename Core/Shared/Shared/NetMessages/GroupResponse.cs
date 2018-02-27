﻿using Shared.NetMessages.GroupMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class GroupResponse : INetMessage
    {
        public List<DbGroup> Groups;
        public List<ErrorMessage> ErrorMessages;
    }
}
