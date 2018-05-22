using Shared;
using Shared.NetMessages.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{
    
    /// <summary>
    /// Interface poskytující zálohovací podporu
    /// </summary>
    public interface ILogger
    {
        //ILogger CreateInstance();
        /// <summary>
        /// Vytvoří log
        /// </summary>
        /// <param name="message">Zpráva</param>
        /// <param name="logType">Závažnost</param>
        void Log(string message, Shared.LogType logType);
        /// <summary>
        /// Odešla logy serveru
        /// </summary>
        /// <typeparam name="T">Druh logu</typeparam>
        /// <param name="logs">Logy</param>
        /// <returns>Odpověď serveru</returns>
        Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> ServerLogAsync<T>(params SLog<T>[] logs) where T : class;
    }
}
