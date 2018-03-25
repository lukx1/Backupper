﻿using System;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.NetMessages;
using System.Net.NetworkInformation;
using Shared.NetMessages.TaskMessages;
using Daemon.Utility;
using System.Net.Http;
using System.Threading;
using Daemon.Logging;
using Shared.LogObjects;

namespace Daemon.Communication
{
    /// <summary>
    /// Jádro daemona
    /// </summary>
    public class DaemonClient
    {
        private Messenger messenger;
        private LoginSettings settings = new LoginSettings();
        private TaskHandler taskHandler = new TaskHandler();
        private Authenticator authenticator;
        private SessionRefresher sessionRefresher;
        private ILogger logger = LoggerFactory.CreateAppropriate();
        private Task TaskTickerTask;

        /// <summary>
        /// Připojí se a začne provádět tasky
        /// </summary>
        public DaemonClient()
        {
            messenger = new Messenger(settings.Server);
        }

        /// <summary>
        /// Vypíše všechno v BRE
        /// </summary>
        /// <param name="e"></param>
        private void DumpErrorMsgs<T>(INetException<T> e, LogType logType = LogType.ERROR)
        {
            logger.Log(e.Message, logType);
            e.ErrorMessages.ForEach(r => logger.Log(r.id + ":" + r.message + "->" + r.value, logType));
        }

        /// <summary>
        /// Pokud je nutno tak se introducne a načte existující tasky
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Startup() //TODO : returnovat log + standardizace logu interface
        {
            authenticator = new Authenticator(messenger);
            if (settings.Uuid == null)// Daemon nema Uuid -> musi se introducnout
            {
                try
                {
                    await authenticator.Introduce();
                }
                catch(INetException<IntroductionResponse> e)
                {
                    logger.Log(
                        $"Nebylo možné se představit{Util.Newline}" +
                        $"Error #1-{e.ErrorMessages[0].id}{Util.Newline}" +
                        $"Příčina : {e.ErrorMessages[0].message}{Util.Newline}" +
                        $"Nelze pokračovat",LogType.CRITICAL
                        );
                    return false;
                }
                catch (FormatException e)
                {
                    logger.Log($"Nebylo možné se představit{Util.Newline}" +
                        $"Error #2-{e.Message}{Util.Newline}" +
                        $"Příčina : {e.StackTrace}{Util.Newline}" +
                        $"Nelze pokračovat",LogType.CRITICAL
                        );
                    return false;
                }
            }
            return true;
        }

        public async Task Run()
        {
            logger.Log("Daemon byl spuštěn", LogType.DEBUG);
            bool canStart = await Startup();
            if (!canStart)
                return;// Nešlo zapnout aplikace a nelze pokračovat
            while (true) // Pokouší se příhlásit dokut se to nepovede
            {
                var guid = await authenticator.AttemptLogin();
                if(guid != Guid.Empty)
                {
                    settings.SessionUuid = guid;
                    settings.Save();
                    break;
                }
                logger.Log("Přihlášení se nepovedlo, bude se opakovat za " + settings.LoginFailureWaitPeriodMs, LogType.ERROR);
                Thread.Sleep(settings.LoginFailureWaitPeriodMs);
            }

            logger.Log("Test odesílání logů...", LogType.DEBUG);
            LogCommunicator logCommunicator = new LogCommunicator(messenger);
            var resp = logCommunicator.SendLog(new DebugLog());
            logger.Log("Log odeslán bez chyby", LogType.DEBUG);

            sessionRefresher = new SessionRefresher(authenticator);// Opakuje login aby session nevyprsel
            sessionRefresher.Run();
            TaskTickerTask = Task.Run(() => TaskTicker());//Refreshuje tasky aby byli aktualni se serverem
        }

        private async Task TaskTicker()
        {
            Thread.CurrentThread.Name = "TaskTicker";
            while (true)
            {
                await LoadTasks();
                logger.Log("Tasky načteny", LogType.INFORMATION);
                Thread.Sleep(settings.TaskRefreshPeriodMs);
            }
        }

        

        private async Task<bool> LoadTasks()
        {
            try
            {
                if (settings.Debug)
                    TaskTest();
                else
                    await ApplyTasks();
                return true;
            }
            catch(INetException<TaskResponse> e)
            {
                logger.Log(
                        $"Nebylo možná načíst tasky{Util.Newline}" +
                        $"Error #2-{e.ErrorMessages[0].id}{Util.Newline}" +
                        $"Příčina : {e.ErrorMessages[0].message}{Util.Newline}",LogType.ERROR
                        );
                return false;
            }
            
        }

        

        /// <summary>
        /// Pro testování tasků
        /// </summary>
        private void TaskTest()
        {
            logger.Log("Probíhá debugovací metoda taskTest", LogType.DEBUG);
            taskHandler.Tasks = Messenger.ReadMessage<TaskResponse>("{\"Tasks\":[{\"id\":1,\"uuidDaemon\":\"50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e\",\"name\":\"DebugTask\",\"description\":\"For debugging\",\"taskLocations\":[{\"id\":1,\"source\":{\"id\":6,\"uri\":\"test.com/docs/imgs\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":4,\"host\":\"test.com\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"destination\":{\"id\":7,\"uri\":\"test.com/backups/imgs\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":5,\"host\":\"test.com\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"backupType\":{\"Id\":1,\"ShortName\":\"NORM\",\"LongName\":\"Normal\"},\"times\":[{\"id\":3,\"interval\":0,\"name\":\"Dneska\",\"repeat\":false,\"startTime\":\""+DateTime.Now.AddSeconds(5)+"\",\"endTime\":\"0001-01-01T00:00:00\"},{\"id\":4,\"interval\":"+5+",\"name\":\"Kazdy Patek\",\"repeat\":true,\"startTime\":\"2018-02-23T00:00:00\",\"endTime\":\"0001-01-01T00:00:00\"}]}]}],\"ErrorMessages\":[]}").Tasks ;
            taskHandler.CreateTimers();
        }

        /// <summary>
        /// Pokud je sezení stále platné
        /// </summary>
        /// <param name="lastCheck"></param>
        /// <returns></returns>
        private bool IsSessionStillValid(DateTime lastCheck)
        {
            return DateTime.Compare(DateTime.Now.AddMinutes(
                settings.SessionLengthMinutes-settings.SessionLengthPaddingMinutes), lastCheck) == -1;
                
        }

        /// <summary>
        /// Získá tasky z db a zapne je
        /// </summary>
        /// <returns></returns>
        private async Task ApplyTasks()
        {
            try
            {
                taskHandler.Tasks = await GetAllTaskFromDB();
                taskHandler.CreateTimers();
            }
            catch(INetException<TaskResponse> e)
            {
                DumpErrorMsgs(e);
            }
        }

        /// <summary>
        /// Fetchuje tasky z db
        /// </summary>
        /// <returns></returns>
        private async Task<List<DbTask>> GetAllTaskFromDB()
        {
            var resp = await messenger.SendAsync<TaskResponse>(new TaskMessage() {sessionUuid = settings.SessionUuid }, "task", HttpMethod.Post);
            sessionRefresher.ExternalyRefreshed();
            return resp.ServerResponse.Tasks;
        }

    }
}
