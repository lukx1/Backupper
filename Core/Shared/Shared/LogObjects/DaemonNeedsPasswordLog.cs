using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class DaemonNeedsPasswordLog : SLog<DaemonNeedsPasswordLog.DaemonNeedsPasswordInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_NEEDS_PASSWORD;

        public override DaemonNeedsPasswordInfo Content { get; protected set; } = new DaemonNeedsPasswordInfo();

        public class DaemonNeedsPasswordInfo
        {
            public string Reason { get; set; }
            public Guid DaemonGuid { get; set; }
        }
    }
}
