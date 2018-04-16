using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class DaemonCrashLog : SLog<DaemonCrashLog.DaemonCrashInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_CRASH;

        public override DaemonCrashInfo Content { get; protected set; } = new DaemonCrashInfo();

        [Serializable]
        public class DaemonCrashInfo
        {
            public string Message { get; set; }
            public Exception CaughtException { get; set; }
            public Guid DaemonUuid { get; set; }
            public string PcInfo { get; set; }
        }

    }
}
