using Daemon.Communication;
using DaemonShared;
using Shared;
using Shared.NetMessages.LogMessages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{

    public class ConsoleLogger : ILogger
    {
        private LoginSettings settings = new LoginSettings();
        private LogCommunicator logCommunicator;

        private static ConsoleLogger logger;


        private ConsoleLogger(Messenger messenger)
        {
            logCommunicator = new LogCommunicator(messenger); 
        }

        private void SetColor(LogType logType)
        {
            switch (logType)
            {
                case LogType.EMERGENCY:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogType.ALERT:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogType.CRITICAL:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogType.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.NOTIFICATION:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case LogType.INFORMATION:
                    break;
                case LogType.DEBUG:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
            }
        }

        private EventLogEntryType Translate(LogType type)
        {
            switch (type)
            {
                case LogType.EMERGENCY:
                case LogType.ALERT:
                case LogType.CRITICAL:
                case LogType.ERROR:
                    return EventLogEntryType.Error;
                case LogType.WARNING:
                case LogType.NOTIFICATION:
                    return EventLogEntryType.Warning;
                case LogType.INFORMATION:
                    return EventLogEntryType.Information;
                case LogType.DEBUG:
                    return EventLogEntryType.Information;
                default:
                    return EventLogEntryType.Error;
            }
        }

        public void Log(string message, LogType logType)
        {
            if ((int)logType > settings.LoggingLevel)
                return;
            if (!Environment.UserInteractive)
            {
                using (EventLog eventLog = new EventLog("Backupper"))
                {
                    eventLog.Source = "Backupper";
                    eventLog.WriteEntry("Log message example", Translate(logType));
                }
            }
            else
            {
                SetColor(logType);
                Console.WriteLine($"{DateTime.Now} - {logType.ToString()} - {message}");
                Console.ResetColor();
            }
        }

        public async Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> ServerLogAsync<T>(params SLog<T>[] logs) where T : class
        {
            foreach (var log in logs)
            {
                Log($"{log.Code.GetType().Name} Server Log", log.LogType);
            }
            return await logCommunicator.SendLog(logs);
        }

        public static ILogger CreateSourceInstance(Messenger messenger)
        {
                return (logger = new ConsoleLogger(messenger));
        }

        public static ILogger CreateInstance()
        {
            if(logger == null)
            {
                throw new InvalidOperationException("Nejdřív musí být zavoláno CreateSourceInstance");
            }
            return logger;
        }
    }
}