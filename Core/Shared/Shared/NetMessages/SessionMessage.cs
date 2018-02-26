using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Zpráva ověřená pomocí sessionUuid
    /// </summary>
    public class SessionMessage : INetMessage
    {
        /// <summary>
        /// Uuid přijaté od serveru
        /// </summary>
        public Guid sessionUuid;
    }
}
