using System;
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
using Shared.NetMessages.LogMessages;

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

        public DaemonClient()
        {
            if (settings.SSLUse)
                messenger = new Messenger(settings.SSLServer, settings.SSLAllowSelfSigned);
            else
                messenger = new Messenger(settings.Server);
        }

        public async Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> CheckForLocalLogsAndSend()
        {
            try
            {
                LogCommunicator logCommunicator = new LogCommunicator(messenger);
                List<JsonableUniversalLog> juls = new List<JsonableUniversalLog>();
                foreach (var jul in new LocalLogManipulator().ReadAllLogs())
                {
                    juls.Add(jul);
                }
                if (juls.Count > 0)
                {
                    logger.Log("Lokální logy nalezeny", LogType.DEBUG);
                    return await logCommunicator.SendLog(juls);
                }
                else
                    logger.Log("Nebyli nalezeny žádné lokální logy", LogType.DEBUG);
            }
            catch(Exception e)
            {
                logger.Log("Chyba při odesílání lokálních logů", LogType.ERROR);
                throw new DoNotStoreThisExceptionException(e.Message, e);
            }
            return null;
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

        private async Task<bool> AttemptLogin(Authenticator authenticator, int tryCount)
        {
            try
            {
                var guid = await authenticator.AttemptLogin();
                if (guid != Guid.Empty)
                {
                    logger.Log("Daemon přihlášen, obdržený session:" + guid, LogType.DEBUG);
                    settings.SessionUuid = guid;
                    settings.Save();
                    return true;
                }
                else//TODO: tohle
                    return false;
            }
            catch (HttpRequestException e)
            {
                logger.Log(
                    $"Server není dostupný{Util.Newline}" +
                    $"Pokus číslo {tryCount + 1}/{settings.LoginMaxRetryCount}{Util.Newline}" +
                    $"Error : {e.Message}{Util.Newline}" +
                    $"Příčina : server není zapnutý nebo odmítá připojení", LogType.WARNING
                    );
                return false;
                
            }
            
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
            int tryCount = 0;
            while (!await AttemptLogin(authenticator,tryCount)) // Pokouší se příhlásit dokut se to nepovede
            {
                if (++tryCount > settings.LoginMaxRetryCount - 1)
                {
                    logger.Log($"Byl dosažen maximální počet pokusů o připojení ({settings.LoginMaxRetryCount}){Util.Newline}Nelze pokračovat...", LogType.CRITICAL);
                    return false;
                }
                logger.Log("Přihlášení se nepovedlo, bude se opakovat za " + TimeSpan.FromMilliseconds(settings.LoginFailureWaitPeriodMs), LogType.WARNING);
                Thread.Sleep(settings.LoginFailureWaitPeriodMs);
            }
            return true;
        }

        public async Task Run()
        {
            logger.Log("Daemon byl spuštěn", LogType.DEBUG);
            bool canStart = await Startup(); // Pokouší se o introduction/login
            if (!canStart)
                return;// Nešlo zapnout aplikace a nelze pokračovat

            /**Už musí být přihlášen**/

            Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> logCheckTask = CheckForLocalLogsAndSend();//FaF

            sessionRefresher = new SessionRefresher(authenticator);// Opakuje login aby session nevyprsel
            sessionRefresher.Run();
            TaskTickerTask = Task.Run(() => TaskTicker());//Refreshuje tasky aby byli aktualni se serverem
            await logCheckTask;
        }

        private async Task TaskTicker()
        {
            Thread.CurrentThread.Name = "TaskTicker";
            while (true)
            {
                logger.Log("Načítání tasků", LogType.DEBUG);
                sessionRefresher.ExternalyRefreshed();
                if(await LoadTasks())
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
