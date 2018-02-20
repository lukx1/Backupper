﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class IntroductionResponse : INetMessage
    {
        public Guid uuid;
        public string password;
        public ErrorMessage errorMessage;
    }
}