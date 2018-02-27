﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class TaskLocationLogResponse : INetMessage
    {
        public List<DbTaskLocationLog> TaskLocationLogs;
        public List<ErrorMessage> ErrorMessages;
    }
}
