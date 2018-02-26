using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class Location
    {
        /// <summary>
        /// Id lokace, při vytváření nemusí být nastaveno
        /// </summary>
        public int id;

        /// <summary>
        /// URI lokace
        /// </summary>
        public string uri;

        /// <summary>
        /// Protokol pomocí kterého přistoupit k lokaci
        /// </summary>
        public Protocol protocol;

        /// <summary>
        /// Přídavné informace k připojení
        /// </summary>
        public LocationCredential LocationCredential;
    }
}
