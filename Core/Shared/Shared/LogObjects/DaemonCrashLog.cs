using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class DaemonCrashLog : ILog<DaemonCrashLog.DaemonCrashInfo>
    {

        public DaemonCrashLog()
        {
            DateCreated = DateTime.Now;
        }

        public int Id { get; set; } = -1;

        public LogType LogType { get; set; } = LogType.ALERT;

        public LogContentType Code => LogContentType.DAEMON_CRASH;

        public DateTime DateCreated { get; set; }

        public DaemonCrashInfo Content { get; private set; } = new DaemonCrashInfo();

        public void Load(JsonableUniversalLog universalLog)
        {
            Id = universalLog.Id;
            LogType = universalLog.LogType;
            DateCreated = universalLog.DateCreated;
            Content = JsonConvert.DeserializeObject<DaemonCrashInfo>(universalLog.Content);
        }

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
