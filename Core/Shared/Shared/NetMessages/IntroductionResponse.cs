﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class IntroductionResponse : INetMessage, INetError
    {
        public Guid uuid { get; set; }
        public string password { get; set; }
        public bool WaitForIntroduction { get; set; } = false;
        public int WaitID { get; set; }

        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}
