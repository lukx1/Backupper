using Daemon.Communication;
using Daemon.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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

        private static void Start()
        {
            try
            {
                DaemonClient daemonClient = new DaemonClient();
                daemonClient.Run().Wait();
            }
            catch(System.Web.HttpException e)
            {
                logger.Log($"Nečekaná http chyba{Util.Newline}" +
                    $"Chyba :{e.Message}{Util.Newline}" +
                    $"ST    :{e.StackTrace}{Util.Newline}" 
                    , LogType.CRITICAL);
                Console.WriteLine("Chcete se aplikaci restartovat (Y/N)");
                while (true)
                {
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Y)
                    {
                        Start();
                        return;
                    }
                    else if (key == ConsoleKey.N)
                        return;
                }
            }
            catch (Exception e)
            {
                DumpErr(e);
                
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var service = new Service();
            if(Environment.UserInteractive)
            {
                service.OnPubStart();
                Console.WriteLine("Press any key to stop");
                Start();
                Console.Read();
                service.OnPubStop();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
