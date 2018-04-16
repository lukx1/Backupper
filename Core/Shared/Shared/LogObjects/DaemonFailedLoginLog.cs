using Newtonsoft.Json;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class DaemonFailedLoginLog : SLog<DaemonFailedLoginLog.DaemonLoginInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_FAILED_LOGIN;

        public override DaemonLoginInfo Content { get; protected set; } = new DaemonLoginInfo();

        public DaemonFailedLoginLog(LogType logType, string UsedAddress, Guid UsedUuid, bool UsedPassword, IEnumerable<ErrorMessage> Errors)
        {
            this.LogType = LogType;
            this.Content.UsedAddress = UsedAddress;
            this.Content.UsedUuid = UsedUuid;
            this.Content.UsedPassword = UsedPassword;
            this.Content.Errors = Errors;
        }

        public class DaemonLoginInfo
        {
            public string UsedAddress { get; set; }
            public Guid UsedUuid { get; set; }
            public bool UsedPassword { get; set; }
            public IEnumerable<ErrorMessage> Errors { get; set; }
        }
    }
}
