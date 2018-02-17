using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class StandardResponseMessage : INetMessage
    {
        /// <summary>
        /// Druh odpovědi
        /// </summary>
        public ResponseType type = ResponseType.UNKNOWN;

        /// <summary>
        /// Lidsky čitelná zpráva
        /// </summary>
        public string message;

        /// <summary>
        /// Vnitřní hodnota
        /// </summary>
        public string value;
        
    }
}
