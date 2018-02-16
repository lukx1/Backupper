using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class SuccessMessage : INetMessage
    {
        public string message;
        public string value;
        public INetMessage innerMessage;
        public uint GetUid()
        {
            return 3;
        }
    }
}
