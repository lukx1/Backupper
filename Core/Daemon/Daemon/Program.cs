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
        

        private static void CatchAnyException()
        {
            if (!AppDomain.CurrentDomain.FriendlyName.EndsWith("vshost.exe"))
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                File.WriteAllText(Path.Combine(Util.GetAppdataFolder(), "crash.log"), e.ToString());
            }
            catch (Exception) { }
        }
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
        /// The main entry point for the application.
        /// </summary>
        static void Main(string []args)
        {
#if OOS
            new Core();
            Thread.Sleep(-1);
#else
            if (!EventLog.SourceExists("Backupper"))
                EventLog.CreateEventSource("Backupper","Backupper");
            CatchAnyException();
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
