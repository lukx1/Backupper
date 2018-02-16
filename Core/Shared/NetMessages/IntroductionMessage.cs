using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class IntroductionMessage : INetMessage
    {
        public string os;
        public char[] macAdress;
        public string preSharedKey;
        public uint version;
    }
}


