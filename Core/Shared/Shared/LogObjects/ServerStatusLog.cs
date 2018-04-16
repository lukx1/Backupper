using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class ServerStatusLog : SLog<ServerStatusLog.ServerStatusInfo>
    {

        public ServerStatusLog(ServerStatusInfo.Status status)
        {
            this.Content.State = status;
            this.DateCreated = DateTime.Now;
        }

        public override LogContentType Code => LogContentType.SERVER_STATUS;

        public override ServerStatusInfo Content { get; protected set; } = new ServerStatusInfo();

        public class ServerStatusInfo
        {
            public enum Status
            {
                STARTING, TURNING_OFF, RESTARTING, SHUTTING_DOWN, EXITING, NO_CHANGE
            }

            public Status State {get;set; }

        }

    }
}
