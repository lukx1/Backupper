﻿using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class TaskResponse : INetMessage
    {
        public List<DbTask> Tasks;
        public List<ErrorMessage> ErrorMessages;
    }
}
