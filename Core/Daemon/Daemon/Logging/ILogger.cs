using Shared;
using Shared.NetMessages.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{
    
    public interface ILogger
    {
        //ILogger CreateInstance();
        void Log(string message, Shared.LogType logType);
        Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> ServerLogAsync<T>(params SLog<T>[] logs) where T : class;
    }
}
