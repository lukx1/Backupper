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

        public async Task<Shared.Messenger.ServerMessage<SpecificLogResponse>> SendLog<T>(params ISpecificLog<T>[] logs)
        {
            List<JsonableSpecificLog> jsonLogs = new List<JsonableSpecificLog>();
            foreach (var log in logs)
            {
                jsonLogs.Add(JsonableSpecificLog.CreateFrom(log));
            }
            return await messenger.SendAsync<SpecificLogResponse>(
                new SpecificLogMessage() { sessionUuid = new LoginSettings().SessionUuid, Logs = jsonLogs },
                "SpecificLog",
                System.Net.Http.HttpMethod.Put
            );
        }

        public async Task<Shared.Messenger.ServerMessage<SpecificLogResponse>> SendLog<T>(params ILog<T>[] logs)
        {
            if(logs is ISpecificLog<T>[])
            {
                return await SendLog((ISpecificLog<T>[])logs);
            }
            List<JsonableUniversalLog> jsonLogs = new List<JsonableUniversalLog>();
            foreach (var log in logs)
            {
                jsonLogs.Add(JsonableUniversalLog.CreateFrom(log));
            }
            return await messenger.SendAsync<SpecificLogResponse>(
                new UniversalLogMessage() { sessionUuid = new LoginSettings().SessionUuid, Logs = jsonLogs },
                "UniversalLog",
                System.Net.Http.HttpMethod.Put
            );
        }

    }
}
