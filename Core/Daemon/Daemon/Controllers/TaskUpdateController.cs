using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.NetMessages;
using Shared.NetMessages.FromDaemon;
using Daemon.Logging;

namespace Daemon.Controllers
{
    [Obsolete]
    public class TaskUpdateController : ListenController
    {
        private ILogger logger = LoggerFactory.CreateAppropriate();

        public override IMarkedNetMessage Receive(string json)
        {
            var msg = base.Deserialize<TaskUpdateMessage>(json);
            logger.Log("Prijata TaskUpdateMessage od serveru",Shared.LogType.DEBUG);
            return new TaskUpdateResponse() { };
        }
    }
}
