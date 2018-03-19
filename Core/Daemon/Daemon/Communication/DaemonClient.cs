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

namespace Daemon.Communication
{
    /// <summary>
    /// Jádro daemona
    /// </summary>
    public class DaemonClient
    {
        private Messenger messenger { get; set; }
        private LoginSettings settings = new LoginSettings();
        private TaskHandler taskHandler = new TaskHandler();
        private Task LoginTickerTask;
        private Task TaskTickerTask;

        /// <summary>
        /// Vypíše všechno v BRE
        /// </summary>
        /// <param name="e"></param>
        private void DumpErrorMsgs(BadResponseException e)
        {
            Console.WriteLine(e.Message);
            e.ErrorMessages.ForEach(r => Console.WriteLine(r.id + ":" + r.message + "->" + r.value));
        }

        private async Task<bool> AttemptLogin()
        {
            try
            {
                await Login(); // Ziska session id
                return true;
            }
            catch (INetException<LoginResponse> e)
            {
                Console.WriteLine("Chyba při pokusu o přihlášení :" + e.Message);
                e.ErrorMessages.ForEach(r => Console.WriteLine(r.id + ":" + r.message + "->" + r.value));
                return false;
            }
        }


        /// <summary>
        /// Pokud je nutno tak se introducne a načte existující tasky
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Startup() //TODO : returnovat log + standardizace logu interface
        {
            if (settings.Uuid == null)// Daemon nema Uuid -> musi se introducnout
            {
                try
                {
                    await Introduce();
                }
                catch(INetException<IntroductionResponse> e)
                {
                    Console.WriteLine(
                        $"Nebylo možné se představit{Util.Newline}" +
                        $"Error #1-{e.ErrorMessages[0].id}{Util.Newline}" +
                        $"Příčina : {e.ErrorMessages[0].message}{Util.Newline}" +
                        $"Nelze pokračovat"
                        );
                    return false;
                }
            }
            return true;
        }

        public async Task Run()
        {
            bool canStart = await Startup();
            if (!canStart) 
                return;// Nešlo zapnout aplikace a nelze pokračovat
            while (! await AttemptLogin()) // Pokouší se příhlásit dokut se to nepovede
            {
                Thread.Sleep(settings.LoginFailureWaitPeriodMs);
            }
            LoginTickerTask = Task.Run(() => LoginTicker());// Opakuje login aby session nevyprsel
            TaskTickerTask = Task.Run(() => TaskTicker());//Refreshuje tasky aby byli aktualni se serverem
        }

        private async Task TaskTicker()
        {
            while (true)
            {
                await LoadTasks();
                if (settings.LoggingLevel >= (int)LogType.DEBUG) //TODO: Tohle
                    Console.WriteLine($"{DateTime.Now} Tasky nacteny");
                Thread.Sleep(settings.TaskRefreshPeriodMs);
            }
        }

        private async Task LoginTicker()
        {
            while (true)
            {
                Thread.Sleep((settings.SessionLengthMinutes - settings.SessionLengthPaddingMinutes)* 60000/*min to ms*/);
                await AttemptLogin();
                if(settings.LoggingLevel >= (int)LogType.DEBUG)//TODO: tohle
                    Console.WriteLine($"{DateTime.Now} Login refreshnut");
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
                Console.WriteLine(
                        $"Nebylo možná načíst tasky{Util.Newline}" +
                        $"Error #2-{e.ErrorMessages[0].id}{Util.Newline}" +
                        $"Příčina : {e.ErrorMessages[0].message}{Util.Newline}"
                        );
                return false;
            }
        }

        /// <summary>
        /// Připojí se a začne provádět tasky
        /// </summary>
        public DaemonClient()
        {
            messenger = new Messenger(settings.Server);
        }

        /// <summary>
        /// Pro testování tasků
        /// </summary>
        private void TaskTest()
        {
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
        /// Zkontroluje jestli je login platný
        /// </summary>
        private void CheckLogin()
        {
            if (settings.SessionUuid == null || !IsSessionStillValid(settings.LastCommunication))
                Login().Wait();
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
            catch(BadResponseException e)
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
            CheckLogin();
            var resp = await messenger.SendAsync<TaskResponse>(new TaskMessage() {sessionUuid = settings.SessionUuid }, "task", HttpMethod.Post);
            return resp.ServerResponse.Tasks;
        }

        private bool IsKeyIdValid(string[] keyId)
        {
            if (keyId == null)
                return false;
            if (keyId.Length != 2)
                return false;
            return int.TryParse(keyId[1], out int a);
        }

        private struct KeyId
        {
            public string Key;
            public int Id;
            public KeyId(string[] parts)
            {
                Key = parts[0];
                Id = int.Parse(parts[1]);
            }
        }

        /// <summary>
        /// Introduces Daemon to the server
        /// </summary>
        public async Task Introduce()
        {
            string firstMacAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            string[] keyId = settings.PreSharedKeyWithIdSemiColSep.Split(';');
            if (!IsKeyIdValid(keyId))
                throw new FormatException("Introduction string nemohl být přečten");
            KeyId kid = new KeyId(keyId);

            IntroductionMessage introductionMessage = new IntroductionMessage()
            {
                preSharedKey = kid.Key,
                id = kid.Id,
                macAdress = firstMacAddress.ToCharArray(),
                os = Environment.OSVersion.ToString(),
                version = Shared.Version.Parse(settings.Version)
            };

            var resp = await messenger.SendAsync<IntroductionResponse>(introductionMessage, "Introduction", System.Net.Http.HttpMethod.Put);
            settings.Uuid = resp.ServerResponse.uuid;
            settings.Password = resp.ServerResponse.password;
        }

        /// <summary>
        /// Přihlásí uživatele a zaznamená si session do configu
        /// </summary>
        public async Task Login()
        {
            LoginMessage loginMessage = new LoginMessage() {password = settings.Password,uuid = settings.Uuid };
            var resp = await messenger.SendAsync<LoginResponse>(loginMessage, "login", HttpMethod.Post);
            settings.SessionUuid = resp.ServerResponse.sessionUuid;
        }
    }
}
