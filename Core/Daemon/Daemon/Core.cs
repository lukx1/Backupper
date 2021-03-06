﻿using Daemon.Communication;
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
    /// <summary>
    /// Základní třída
    /// </summary>
    public class Core
    {

        private static ILogger logger = UniLogger.CreateSourceInstance(null);
        private static DaemonClient DaemonClient;
        private static Task PipeListener;

        /// <summary>
        /// Dumpne chyby do konzole
        /// </summary>
        /// <param name="e"></param>
        private static void DumpErr(Exception e)
        {
            logger.Log($"Nečekaná chyba{Environment.NewLine}" +
                    $"Chyba :{e.Message}{Environment.NewLine}" +
                    $"ST    :{e.StackTrace}{Environment.NewLine}"
                    , LogType.EMERGENCY);

            if (e.InnerException != null)
                DumpErr(e.InnerException);
        }

        /// <summary>
        /// Pokusí se ukázat lokální popup
        /// </summary>
        /// <returns>True pokud úspěšno, false jinak</returns>
        private static bool LocalErrorPopup(LocalException e)
        {
            try
            {
                var settings = new LoginSettings();
                if (!settings.PipeOK)
                    return false;
                PipeComs pipeComs = new PipeComs();

                var task = pipeComs.SendMessageAsync(new PipeMessage() { Code = PipeCode.POPUP_ERR, SerializePayload = e.Popup });
                task.Wait();
            }
            catch (Exception e2)
            {
                if (!(e2 is LocalException))
                    LogCrash(e2);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Záznam a případně log chyb
        /// </summary>
        /// <param name="e"></param>
        private static void LogCrash(Exception e)
        {
            DumpErr(e);
            if (e is LocalException)
            {
                if (LocalErrorPopup((LocalException)e))
                    return;

            }
            if ((e is DoNotStoreThisExceptionException))
                return;
            try
            {
                LocalLogManipulator logManipulator = new LocalLogManipulator();
                DaemonCrashLog crashLog = new DaemonCrashLog();
                crashLog.Content.CaughtException = e;
                crashLog.Content.Message = "Nečekaná chyba";
                crashLog.Content.PcInfo = "";
                crashLog.Content.DaemonUuid = new LoginSettings().Uuid;
                logManipulator.Store(crashLog);
            }
            catch (Exception ex)
            {
                logger.Log($"Chyba při tvoření záznamu o chybě\r\n{ex.Message}\r\n{ex.StackTrace}", LogType.EMERGENCY);
            }
        }



        /// <summary>
        /// Zapne novy asynchroni thread DS
        /// </summary>
        private async static void NewThreadDS()
        {
            var settings = new LoginSettings();
            settings.PipeOK = false;

            var pipes = new PipeComs();

            logger.Log("NamedPipe - Začíná pokus o připojení", LogType.DEBUG);
            try
            {
                await pipes.SendMessageAsync(new PipeMessage() { Code = PipeCode.NOTIFY_WAITER, SerializePayload = new PipeServiceIdentity() { Identity = WindowsIdentity.GetCurrent().Owner.ToString() } });
                logger.Log("NamedPipe - Úspěšně připojeno", LogType.DEBUG);
                settings.PipeOK = true;
            }
            catch (IOException e)
            {
                logger.Log($"NamedPipe - IO chyba\r\n{e.StackTrace}", LogType.WARNING);
                var log = new GeneralDaemonError() { };
                log.LogType = LogType.WARNING;
                log.Content.DaemonUuid = new LoginSettings().Uuid;
                log.Content.Message = $"NamedPipe - IO chyba\r\n{e.StackTrace}";
                try
                {
                    await logger.ServerLogAsync(log);
                }
                catch (Exception ex)
                {
                    DumpErr(ex);
                }
            }
            catch (System.TimeoutException)
            {
                logger.Log("NamedPipe - Vypršel čas na připojení", LogType.WARNING);
                settings.PipeOK = false;
                return;
            }
            finally
            {
                settings.Save();
            }

            if (settings.LoggingLevel >= (int)LogType.DEBUG)
                PipeComs.MessageReceived += Pipes_MessageReceived;
            PipeListener = Task.Run(() => pipes.StartListening());

            logger.Log("NamedPipe - Čtení připraveno", LogType.DEBUG);
        }

        /// <summary>
        /// Debug zpráva přijata
        /// </summary>
        /// <param name="msg"></param>
        private static void Pipes_MessageReceived(PipeMessage msg)
        {
            logger.Log($"NamedPipe - Přijata zpráva {msg.Code}{(msg.Payload.Length > 0 ? "-" : "")}{msg.Payload}", LogType.DEBUG);
        }

        /// <summary>
        /// Zapne daemon settings
        /// </summary>
        private static void StartDS()
        {
            NewThreadDS();
        }

        /// <summary>
        /// Metoda zapne Daemona a umožní logování
        /// </summary>
        private static void Start()
        {
            try
            {
                //throw new ArgumentException("Test excep",new Exception("Test error please ignore", new Exception("Test inner", new Exception("Test inner 2"))));
                DaemonClient = new DaemonClient();
                logger = UniLogger.CreateSourceInstance(DaemonClient.messenger);
                DaemonClient.Run().Wait();
            }
            catch (Exception e)
            {
                LogCrash(e.InnerException);
                Thread.Sleep(60000);
                Start();
            }
        }

        /// <summary>
        /// Po prodlevě spustí daemona
        /// </summary>
        public Core()
        {
            //Thread.Sleep(10000);
            //StartDS();
            Start();
        }
    }
}
