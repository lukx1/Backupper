using Daemon.Communication;
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
                DaemonClient daemonClient = new DaemonClient();
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
