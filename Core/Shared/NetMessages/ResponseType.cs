using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public enum ResponseType
    {
        UNKNOWN,
        YES,
        NO,
        /// <summary>
        /// Chyba
        /// </summary>
        ERROR,
        /// <summary>
        /// Proces úspěšný
        /// </summary>
        SUCCESS,
        /// <summary>
        /// Proces selhal
        /// </summary>
        FAILURE
    }
}
