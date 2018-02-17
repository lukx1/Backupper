using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Testuje odezvu
    /// </summary>
    public class PingMessage : INetMessage
    {
        /// <summary>
        /// Začátek měření, nastaven klientem
        /// </summary>
        public long startTime { get; set; } 
        /// <summary>
        /// Výsledek měření, nstaven serverem
        /// </summary>
        public long endTime { get; set; }
    }
}
