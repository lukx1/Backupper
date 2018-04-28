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

namespace Daemon
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string []args)
        {
#if OOS
            new Core();
            Thread.Sleep(-1);
#else
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
