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
namespace Daemon
{
    static class Program
    {
        private static ILogger logger = LoggerFactory.CreateAppropriate();

        private static void DumpErr(Exception e)
        {
            logger.Log($"Nečekaná chyba{Util.Newline}" +
                    $"Chyba :{e.Message}{Util.Newline}" +
                    $"ST    :{e.StackTrace}{Util.Newline}"
                    , LogType.EMERGENCY);

            if (e.InnerException != null)
                DumpErr(e.InnerException);
        }

        private static void LogCrash(Exception e)
        {//TODO: popup
            DumpErr(e);
            if ((e is DoNotStoreThisExceptionException))
                return;
            LocalLogManipulator logManipulator = new LocalLogManipulator();
            DaemonCrashLog crashLog = new DaemonCrashLog();
            crashLog.Content.CaughtException = e;
            crashLog.Content.Message = "Nečekaná chyba";
            crashLog.Content.PcInfo = "";
            crashLog.Content.DaemonUuid = new LoginSettings().Uuid;
            logManipulator.Store(crashLog);
        }

        private static void Start()
        {
            try
            {
                //throw new ArgumentException("Test excep",new Exception("Test error please ignore", new Exception("Test inner", new Exception("Test inner 2"))));
                DaemonClient daemonClient = new DaemonClient();
                daemonClient.Run().Wait();
            }
            catch (Exception e)
            {
                LogCrash(e);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var service = new Service();
            if (Environment.UserInteractive)
            {
                service.OnPubStart();
                Console.WriteLine("Press any key to stop");
                Start();
                Console.Read();
                service.OnPubStop();
            }
            else //TODO: Try catch wrapper
            {
                ServiceBase.Run(service);
            }
        }
    }
}
