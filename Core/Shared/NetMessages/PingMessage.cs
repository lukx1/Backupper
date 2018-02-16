using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class PingMessage : INetMessage
    {
        /**In ms*/
        public long startTime { get; set; } 
        /**In ms*/
        public long endTime { get; set; }
    }
}
