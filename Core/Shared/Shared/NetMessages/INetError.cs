﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public interface INetError
    {
        List<ErrorMessage> ErrorMessages { get; }
    }
}
