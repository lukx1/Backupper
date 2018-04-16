using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class DaemonFailedIntroductionLog : SLog<DaemonFailedIntroductionLog.DaemonFailedIntroductionInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_FAILED_INTRO;

        public override DaemonFailedIntroductionInfo Content { get; protected set; } = new DaemonFailedIntroductionInfo();

        public class DaemonFailedIntroductionInfo
        {
            public Guid UsedCode { get; set; }
            public int UsedID { get; set; }
        }
    }
}
