using Daemon.Communication;
using Daemon.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Daemon.Backups;
using Daemon.Utility;
using Shared.LogObjects;
using DaemonShared;
using DaemonShared.Pipes;
using System.Security.Principal;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Daemon
{
    static class Program
    {
        

        /// <summary>
        /// Zaznamenává nechycené exceptiony
        /// </summary>
        /// <param name="sender">Odesílatel</param>
        /// <param name="e">Exception</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                File.WriteAllText(Path.Combine(Util.GetAppdataFolder(), "crash.log"), e.ToString());
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Překlad logType na windows EventLogEntryType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static EventLogEntryType Translate(LogType type)
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

        /// <summary>
        /// Vstup pro aplikaci
        /// </summary>
        static void Main(string []args)
        {
#if OOS
            //Pro VS debugovani
            new Core();
            Thread.Sleep(-1);
#else
            if (!EventLog.SourceExists("Backupper"))
                EventLog.CreateEventSource("Backupper","Backupper");
            if (Environment.UserInteractive)
            {
                Service service = new Daemon.Service();
                service.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] serviceBase = new ServiceBase[] { new Daemon.Service() };
                ServiceBase.Run(serviceBase);
            }
#endif
        }
    }
}
