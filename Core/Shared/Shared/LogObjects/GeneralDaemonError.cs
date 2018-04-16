using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class GeneralDaemonError : SLog<GeneralDaemonError.GeneralDaemonInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_GENERAL_ERROR;

        public override GeneralDaemonInfo Content { get; protected set; } = new GeneralDaemonInfo();

        public class GeneralDaemonInfo
        {
            public Guid DaemonUuid { get; set; }
            public string Message { set; get; }
        }
    }
}
