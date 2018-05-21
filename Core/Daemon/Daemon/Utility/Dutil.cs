using DaemonShared;
using Shared;
using Shared.LogObjects;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Utility
{
    /// <summary>
    /// Pomocné funkce pro damenoa
    /// </summary>
    public static class Dutil
    {
        /// <summary>
        /// Pomocná funkce pro GSRL
        /// </summary>
        /// <param name="LogType">Druh logu</param>
        /// <param name="errors">Chyby</param>
        /// <returns></returns>
        public static GeneralServerResponseLog CreateGSRL(LogType LogType, IEnumerable<ErrorMessage> errors)
        {
            var v = new GeneralServerResponseLog()
            {
                LogType = LogType,

            };
            v.Content.DaemonUuid = new LoginSettings().Uuid;
            v.Content.Errors = errors;
            return v;
        }
    }
}
