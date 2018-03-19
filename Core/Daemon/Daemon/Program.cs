using Daemon.Communication;
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
                try
                {
                    DaemonClient daemonClient = new DaemonClient();
                    daemonClient.Run().Wait();
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Nečekaná chyba{Util.Newline}" +
                        $"Chyba :{e.Message}{Util.Newline}" +
                        $"ST    :{e.StackTrace}{Util.Newline}" +
                        $"Aplikace nemůže pokračovat");
                    return;
                    //Logger.log
                }
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
