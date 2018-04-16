using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class GeneralTaskFailedLog : SLog<GeneralTaskFailedLog.GeneralTaskFailedInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_FAILED_TASK_GENERAL;

        public override GeneralTaskFailedInfo Content { get; protected set; } = new GeneralTaskFailedLog.GeneralTaskFailedInfo();

        public GeneralTaskFailedLog(int TaskID, Exception exception, DbTime time)
        {
            this.Content.TaskID = TaskID;
            this.Content.CaughtException = exception;
            this.Content.TaskTime = time;
        }

        public class GeneralTaskFailedInfo
        {
            public int TaskID { get; set; }
            public Exception CaughtException { get; set; }
            public DbTime TaskTime { get; set; }
        }
    }
}
