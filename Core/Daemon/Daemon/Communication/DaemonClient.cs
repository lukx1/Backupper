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
using DaemonShared;
using DaemonShared.Pipes;
using Daemon.Data;
using System.IO;

namespace Daemon.Communication
{
    /// <summary>
    /// Jádro daemona
    /// </summary>
    public class DaemonClient
    {
        public Messenger messenger { get; private set; }
        private LoginSettings settings = new LoginSettings();
        private TaskHandler taskHandler = new TaskHandler();
        private Authenticator authenticator;
        private SessionRefresher sessionRefresher;
        private ILogger logger;
        private Task TaskTickerTask;

        /// <summary>
        /// Zabije všechny běžící thready a tasky
        /// </summary>
        public void Kill()
        {
            taskHandler.Dispose();
            sessionRefresher.Dispose();
            TaskTickerTask.Dispose();
        }

        /// <summary>
        /// Vytvoří DC a založí messengera a logger
        /// </summary>
        public DaemonClient()
        {
            
            if (settings.SSLUse)
                messenger = new Messenger(settings.SSLServer, settings.SSLAllowSelfSigned);
            else
                messenger = new Messenger(settings.Server);
            logger = ConsoleLogger.CreateSourceInstance(messenger);
        }

        /// <summary>
        /// Zkontroluje pokud jsou lokákní zálohy a poté je odešlě serveru
        /// </summary>
        /// <returns>Odpověď serveru</returns>
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
            catch (Exception e)
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
            var faf = logger.ServerLogAsync(Dutil.CreateGSRL(LogType.ERROR, e.ErrorMessages));
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
                else
                {
                    throw new LocalException("Neočekávaná chyba\r\nDaemon se přihlásil ale obdržil prázdný session\r\nNelze pokračovat");
                }
            }
            catch (HttpRequestException e)
            {
                logger.Log(
                    $"Server není dostupný{Environment.NewLine}" +
                    $"Pokus číslo {tryCount + 1}/{settings.LoginMaxRetryCount}{Environment.NewLine}" +
                    $"Error : {e.Message}{Environment.NewLine}" +
                    $"Příčina : server není zapnutý nebo odmítá připojení", LogType.WARNING
                    );
                return false; 

            }

        }

        private void firstSetup()
        {
            var dir = Path.Combine(Util.GetSharedFolder(), "Install");
            var file = Path.Combine(dir, "transfer.tsf");
            string[] res = File.ReadAllText(file).Split(';');
            string pass = res[0];
            string user = res[1];
            string priv = res[2];
            string serv = res[3];
            settings.Reset();
            settings.FirstSetup = false;
            settings.OwnerUserNickname = user;
            settings.RSAPrivate = priv;
            settings.Server = serv;
            File.Delete(file);
            settings.Save(); 

        }

        /// <summary>
        /// Pokud je nutno tak se introducne a načte existující tasky
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Startup()
        {
            authenticator = new Authenticator(messenger);
            if (settings.FirstSetup)
                firstSetup();
            if (settings.Uuid == null || settings.Uuid == Guid.Empty)// Daemon nema Uuid -> musi se introducnout
            {
                try
                {
                    await authenticator.Introduce();
                }
                catch (INetException<IntroductionResponse> e)
                {
                    logger.Log(
                        $"Nebylo možné se představit{Environment.NewLine}" +
                        $"Error #1-{e.ErrorMessages[0].id}{Environment.NewLine}" +
                        $"Příčina : {e.ErrorMessages[0].message}{Environment.NewLine}" +
                        $"Nelze pokračovat", LogType.CRITICAL
                        );
                    throw new LocalException("Nelze se introducnout", e);
                }
                catch (FormatException e)
                {
                    logger.Log($"Nebylo možné se představit{Environment.NewLine}" +
                        $"Error #2-{e.Message}{Environment.NewLine}" +
                        $"Příčina : {e.StackTrace}{Environment.NewLine}" +
                        $"Nelze pokračovat", LogType.CRITICAL
                        );
                    throw new LocalException("Nelze se introducnout - špatný formát", e);
                }
            }
            int tryCount = 0;
            while (!await AttemptLogin(authenticator, tryCount)) // Pokouší se příhlásit dokut se to nepovede
            {
                if ((++tryCount > settings.LoginMaxRetryCount - 1) && settings.LoginMaxRetryCount != -1)
                {
                    logger.Log($"Byl dosažen maximální počet pokusů o připojení ({settings.LoginMaxRetryCount}){Environment.NewLine}Nelze pokračovat...", LogType.CRITICAL);
                    throw new LocalException($"Nelze kontaktovat server {(settings.SSLUse ? settings.SSLServer : settings.Server)}");
                }
                logger.Log("Přihlášení se nepovedlo, bude se opakovat za " + TimeSpan.FromMilliseconds(settings.LoginFailureWaitPeriodMs+(60000*tryCount)), LogType.WARNING);
                Thread.Sleep(settings.LoginFailureWaitPeriodMs);
            }
            return true;
        }

        private async Task TryLoadPrivateKey()
        {
            if (settings.RSAPrivate == null || settings.RSAPrivate.Trim() == "")
            {
                logger.Log("Daemon nemá soukromý klíč a nemůže zjistit údaje pro vzdálené lokace(FTP,SMTP či SMB)", LogType.WARNING);
                var log = new DaemonNeedsPasswordLog() { LogType = LogType.WARNING };
                log.Content.DaemonGuid = settings.Uuid;
                log.Content.Reason = "Daemon vyžaduje klíč vlastníka pro získání přístupu ke vzdáleným lokacím";
                var faf = await logger.ServerLogAsync(log);
                PipeComs.MessageReceived += PipeComs_MessageReceivedUserLogin;

            }
        }

        private async Task DecodePK(string pass)
        {
            try
            {
                var result = await messenger.SendAsync<RSAForDaemonResponse>(new RSAForDaemonMessage() { sessionUuid = settings.SessionUuid }, "RSAForDaemon", HttpMethod.Post);
                var newSettings = new LoginSettings();
                newSettings.RSAPrivate = PasswordFactory.DecryptAES(result.ServerResponse.EncryptedPrivateKey,pass);
                newSettings.Save();
            }
            catch (INetException<RSAForDaemonResponse> ex)
            {
                logger.Log("Chyba při pokusu získání soukromého klíče pro dešifrování hesel", LogType.ERROR);
                var log = new GeneralServerResponseLog() { LogType = LogType.ERROR };
                log.Content.DaemonUuid = settings.Uuid;
                log.Content.StatusCode = ex.ServerResponse.StatusCode;
                log.Content.Errors = ex.ErrorMessages ?? new List<ErrorMessage>();
                var faf = logger.ServerLogAsync(log);
            }
        }

        private async void PipeComs_MessageReceivedUserLogin(DaemonShared.Pipes.PipeMessage msg)
        {

            if (msg.Code != DaemonShared.Pipes.PipeCode.USER_LOGIN)
                return;
            logger.Log("NamedPipe - Byl přijat požadavek o přihlášení\r\n", LogType.DEBUG);
            try
            {
                var obj = msg.DeserializePayload<PipeLoginAttempt>();
                var res = await messenger.SendAsync<UserLoginResponse>(new UserLoginMessage() { Password = obj.P, Username = obj.U }, "UserLogin", HttpMethod.Post);
                PipeComs coms = new PipeComs();
                var respTask =  coms.SendMessageAsync(new PipeMessage() { Code = PipeCode.LOGIN_RESPONSE, SerializePayload = new PipeLoginResponse() { B = true } });
                await DecodePK(obj.P);
                await respTask;
            }
            catch (INetException<LoginResponse>)
            {
                logger.Log("NamedPipe - pokus o přihlášení uživatele se nezdařil", LogType.WARNING);
                var log = new GeneralDaemonError() { LogType = LogType.WARNING };
                log.Content.DaemonUuid = settings.Uuid;
                log.Content.Message = "Manuální pokus o přihlášení uživatele byl neúspěšný";
                var taskL = logger.ServerLogAsync(log);
                PipeComs coms = new PipeComs();
                await coms.SendMessageAsync(new PipeMessage() { Code = PipeCode.LOGIN_RESPONSE, SerializePayload = new PipeLoginResponse() { B = true } });
                await taskL;
            }
        }

        /// <summary>
        /// Zapne Daemona
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            logger.Log("Daemon byl spuštěn", LogType.DEBUG);
            bool canStart = await Startup(); // Pokouší se o introduction/login
            if (!canStart)
                return;// Nešlo zapnout aplikace a nelze pokračovat

            /**Už musí být přihlášen**/

            var loadPrivTask = TryLoadPrivateKey();

            Task<Shared.Messenger.ServerMessage<UniversalLogResponse>> logCheckTask = CheckForLocalLogsAndSend();//FaF

            sessionRefresher = new SessionRefresher(authenticator);// Opakuje login aby session nevyprsel
            sessionRefresher.Run();
            TaskTickerTask = Task.Run(() => TaskTicker());//Refreshuje tasky aby byli aktualni se serverem
            await logCheckTask;
            await loadPrivTask;
        }

        private async Task TaskTicker()
        {
            Thread.CurrentThread.Name = "TaskTicker";
            while (true)
            {
                logger.Log("Načítání tasků", LogType.DEBUG);
                sessionRefresher.ExternalyRefreshed();
                try
                {
                    if (await LoadTasks())
                        logger.Log("Tasky načteny", LogType.INFORMATION);
                }
                catch(Exception e)
                {

                    logger.Log(e.StackTrace,LogType.ERROR);
                    GeneralDaemonError log = new GeneralDaemonError() {LogType = LogType.ERROR };
                    log.Content.DaemonUuid = settings.Uuid;
                    log.Content.Message = "Chyba při pokusu o znovunačtení tasků\r\n"+e.ToString();
                    var faf = logger.ServerLogAsync(log);
                }
                Thread.Sleep(settings.TaskRefreshPeriodMs);
            }
        }



        private async Task<bool> LoadTasks()
        {
            try
            {
                if (settings.Debug)
                    await TaskTest();
                else
                    await ApplyTasks();
                return true;
            }
            catch (INetException<TaskResponse> e)
            {
                logger.Log(
                        $"Nebylo možná načíst tasky{Environment.NewLine}" +
                        $"Error #2-{e.ErrorMessages[0].id}{Environment.NewLine}" +
                        $"Příčina : {e.ErrorMessages[0].message}{Environment.NewLine}", LogType.ERROR
                        );
                var faf = logger.ServerLogAsync(Dutil.CreateGSRL(LogType.ERROR, e.ErrorMessages));
                return false;
            }

        }



        /// <summary>
        /// Pro testování tasků
        /// </summary>
        private async Task TaskTest()
        {
            logger.Log("Probíhá debugovací metoda taskTest", LogType.DEBUG);
            //taskHandler.Tasks = Messenger.ReadMessage<TaskResponse>("{\"Tasks\":[{\"id\":1,\"uuidDaemon\":\"50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e\",\"name\":\"DebugTask\",\"description\":\"For debugging\",\"taskLocations\":[{\"id\":1,\"source\":{\"id\":6,\"uri\":\"test.com/docs/imgs\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":4,\"host\":\"test.com\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"destination\":{\"id\":7,\"uri\":\"test.com/backups/imgs\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":5,\"host\":\"test.com\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"backupType\":{\"Id\":1,\"ShortName\":\"NORM\",\"LongName\":\"Normal\"},\"times\":[{\"id\":3,\"interval\":0,\"name\":\"Dneska\",\"repeat\":false,\"startTime\":\"" + DateTime.Now.AddSeconds(5) + "\",\"endTime\":\"0001-01-01T00:00:00\"},{\"id\":4,\"interval\":" + 5 + ",\"name\":\"Kazdy Patek\",\"repeat\":true,\"startTime\":\"2018-02-23T00:00:00\",\"endTime\":\"0001-01-01T00:00:00\"}]}]}],\"ErrorMessages\":[]}").Tasks;
            try
            {
                taskHandler.Tasks = (await messenger.SendAsync<TaskResponse>(new TaskMessage() { IsDaemon = true, sessionUuid = settings.SessionUuid, tasks = new List<DbTask>() { new DbTask() { id = -1 }, new DbTask() { id = -2 }, new DbTask() { id = -3 } } }, "Task", HttpMethod.Post)).ServerResponse.Tasks;
                taskHandler.CreateTimers();
            }
            catch(INetException<TaskResponse> ex)
            {
                logger.Log("Chyba při testování tasků v TaskTest\r\nTato chyba nebude zaznamenána!", LogType.ERROR);
                logger.Log($"{ex.Message}\r\n{ex.StackTrace}",LogType.ERROR);
            }
        }

        /// <summary>
        /// Pokud je sezení stále platné
        /// </summary>
        /// <param name="lastCheck"></param>
        /// <returns></returns>
        private bool IsSessionStillValid(DateTime lastCheck)
        {
            return DateTime.Compare(DateTime.Now.AddMinutes(
                settings.SessionLengthMinutes - settings.SessionLengthPaddingMinutes), lastCheck) == -1;

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
            catch (INetException<TaskResponse> e)
            {
                var faf = logger.ServerLogAsync(Dutil.CreateGSRL(LogType.CRITICAL, e.ErrorMessages));
                var log = new GeneralDaemonError() { LogType = LogType.CRITICAL};
                DumpErrorMsgs(e);
            }
        }

        /// <summary>
        /// Fetchuje tasky z db
        /// </summary>
        /// <returns></returns>
        private async Task<List<DbTask>> GetAllTaskFromDB()
        {
            TaskKeeper keeper = new TaskKeeper(messenger);
            return await keeper.GetAppropriate(settings.SessionUuid);
            /*var resp = await messenger.SendAsync<TaskResponse>(new TaskMessage() { sessionUuid = settings.SessionUuid }, "task", HttpMethod.Post);
            sessionRefresher.ExternalyRefreshed();
            var tasks = resp.ServerResponse.Tasks;
            return tasks;*/
        }

    }
}
