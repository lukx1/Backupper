using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class DaemonTaskSuccessLog : SLog<DaemonTaskSuccessLog.DaemonTaskSuccessInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_TASK_SUCCESS;

        public override DaemonTaskSuccessInfo Content { get; protected set; } = new DaemonTaskSuccessInfo();

        public class DaemonTaskSuccessInfo
        {
            public int TaskID { get; set; }
            public DateTime TimeStarted { get; set; }
            public DateTime TimeFinished { get; set; }
            public string Notes { get; set; }
        }

    }
}
