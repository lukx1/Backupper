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
    public static class Dutil
    {
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
