using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Zachytí jakoukoliv obecnou ano/ne odpověď serveru
    /// </summary>
    public class CommonResponseMessage : INetMessage
    {
        /// <summary>
        /// Lidsky čitelná zpráva
        /// </summary>
        public string message;

        /// <summary>
        /// Vnitřní hodnota
        /// </summary>
        public string value;

        /// <summary>
        /// Extra data která byla přijata
        /// </summary>
        [JsonExtensionData]
        private IDictionary<string, JToken> _extraStuff;
    }
}
