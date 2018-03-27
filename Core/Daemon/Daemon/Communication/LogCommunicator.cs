using Shared;
using Shared.NetMessages.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Daemon.Communication
{
    public class LogCommunicator
    {

        private Messenger messenger;

        public LogCommunicator(Messenger messenger)
        {
            this.messenger = messenger;
        }

        public async Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> SendLog<T>(params ILog<T>[] logs) where T : class
        {
            List<JsonableUniversalLog> jsonLogs = new List<JsonableUniversalLog>();
            foreach (var log in logs)
            {
                jsonLogs.Add(JsonableUniversalLog.CreateFrom(log));
            }
            return await messenger.SendAsync<UniversalLogResponse>(
                new UniversalLogMessage() { sessionUuid = new LoginSettings().SessionUuid, Logs = jsonLogs },
                "UniversalLog",
                System.Net.Http.HttpMethod.Put
            );
        }

    }
}
