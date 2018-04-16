using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class GeneralServerResponseLog : SLog<GeneralServerResponseLog.GeneralServerResponseInfo>
    {
        public override LogContentType Code => LogContentType.DAEMON_GENERAL_SERVER_RESPONSE;

        public override GeneralServerResponseInfo Content { get; protected set; } = new GeneralServerResponseInfo();

        public class GeneralServerResponseInfo
        {
            public Guid DaemonUuid { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public IEnumerable<ErrorMessage> Errors { get; set; }
        }
    }
}
