using DaemonShared;
using Shared;
using Shared.NetMessages.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Daemon.Communication
{
    /// <summary>
    /// Usnadňuje odesílání logu serveru
    /// </summary>
    public class LogCommunicator
    {

        private Messenger messenger;

        public LogCommunicator(Messenger messenger)
        {
            this.messenger = messenger;
        }
        
        /// <summary>
        /// Odešle logy serveru
        /// </summary>
        /// <param name="logs">Logy</param>
        /// <returns>Odpověď serveru</returns>
        public async Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> SendLog(IEnumerable<JsonableUniversalLog> logs)
        {
            return await SendLog(logs.ToArray());
        }

        /// <summary>
        /// Odešle logy serveru
        /// </summary>
        /// <param name="logs">Logy</param>
        /// <returns>Odpověď serveru</returns>
        public async Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> SendLog(params JsonableUniversalLog[] logs)
        {
            return await messenger.SendAsync<UniversalLogResponse>(
                new UniversalLogMessage() { sessionUuid = new LoginSettings().SessionUuid, Logs = logs },
                "UniversalLog",
                System.Net.Http.HttpMethod.Put
            );
        }
        /// <summary>
        /// Odešle logy serveru
        /// </summary>
        /// <param name="logs">Logy</param>
        /// <returns>Odpověď serveru</returns>
        public async Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> SendLog<T>(params SLog<T>[] logs) where T : class
        {
            List<JsonableUniversalLog> jsonLogs = new List<JsonableUniversalLog>();
            foreach (var log in logs)
            {
                jsonLogs.Add(JsonableUniversalLog.CreateFrom(log));
            }
            return await SendLog(jsonLogs);
        }

    }
}
