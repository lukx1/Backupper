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

namespace Daemon
{
    static class Program
    {
        private static ILogger logger = ConsoleLogger.CreateSourceInstance(null);
        private static Service service;
        private static DaemonClient DaemonClient;
        private static PipeComs pipes;

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



        private async static void NewThreadDS()
        {
            pipes.MessageReceived += Pipes_MessageReceived;
            //var faf = Task.Run(async () => await pipes.StartListeningAsync());
            
            logger.Log("NamedPipe - Začíná pokus o připojení", LogType.DEBUG);
            try
            {
                await pipes.SendMessageAsync(new PipeMessage() { Code = PipeCode.NOTIFY_WAITER });
                logger.Log("NamedPipe - Úspěšně připojeno", LogType.DEBUG);
            }
            catch (System.TimeoutException)
            {
                logger.Log("NamedPipe - Vypršel čas na připojení", LogType.WARNING);
                return;
            }

            await pipes.SendMessageAsync(new PipeMessage() { Code = PipeCode.POPUP_ERR, Payload =new PipePopup() {B = "Test",  C="Backupper",I = PipePopup.MessageBoxIconsP.Question,T = PipePopup.MessageBoxButtonsP.OK }.ToJsonZip() });

            logger.Log("NamedPipe - Čtení připraveno", LogType.DEBUG);

        }

        private static void Pipes_MessageReceived(PipeMessage msg)
        {
            logger.Log($"NamedPipe - Přijata zpráva {msg.Code}-{msg.Payload}", LogType.DEBUG);
        }

        private static void StartDS()
        {
            pipes = new PipeComs();
            NewThreadDS();
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
            service = new Service();
            service.ServiceName = "Backupper";
            if (Environment.UserInteractive)
            {
                service.OnPubStart();
                Console.WriteLine("Press any key to stop");
                StartDS();
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
