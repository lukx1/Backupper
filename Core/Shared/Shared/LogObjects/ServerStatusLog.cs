using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class ServerStatusLog : ILog<ServerStatusLog.ServerStatusInfo>
    {

        public ServerStatusLog(ServerStatusInfo.Status status)
        {
            this.Content.State = status;
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        public LogType LogType { get; set; } = LogType.INFORMATION;

        public LogContentType Code => LogContentType.SERVER_STATUS;

        public DateTime DateCreated { get; set; }

        public ServerStatusInfo Content { get; set; } = new ServerStatusInfo();

        public void Load(JsonableUniversalLog universalLog)
        {
            this.Id = universalLog.Id;
            this.DateCreated = universalLog.DateCreated;
            this.LogType = universalLog.LogType;
            this.Content = JsonConvert.DeserializeObject<ServerStatusLog.ServerStatusInfo>(universalLog.Content);
        }

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
