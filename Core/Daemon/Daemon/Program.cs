using Daemon.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Daemon.Backups;

namespace Daemon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //var service = new Service1();
            //if(Environment.UserInteractive)
            //{
            //    service.OnPubStart();
            //    Console.WriteLine("Press any key to stop");
            //    DaemonClient daemonClient = new DaemonClient();
            //    Console.Read();
            //    service.OnPubStop();
            //}
            //else
            //{
            //    ServiceBase.Run(service);
            //}

            Backup test = new Backup(0,BackupType.NORM);
            test.Zipped = true;
            test.AddSource("C:/Users/rambo_000/Desktop/TESTFOLDER/TestBackup");
            test.AddDestination("C:/Users/rambo_000/Desktop/TESTFOLDER/BACKUPS");
            test.AddDestination("C:/Users/rambo_000/Desktop/TESTFOLDER/TestDestination");
            test.BackupAll();
            Console.WriteLine("DONE");
            Console.Read();
        }
    }
}
