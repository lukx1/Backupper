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

namespace Daemon.Communication
{
    /// <summary>
    /// Jádro daemona
    /// </summary>
    public class DaemonClient
    {
        private Messenger messenger { get; set; }
        private IConfig config = new DynamicConfig();
        private TaskHandler taskHandler = new TaskHandler();

        /// <summary>
        /// Vypíše všechno v BRE
        /// </summary>
        /// <param name="e"></param>
        private void DumpErrorMsgs(BadResponseException e)
        {
            Console.WriteLine(e.Message);
            e.ErrorMessages.ForEach(r => Console.WriteLine(r.id + ":" + r.message + "->" + r.value));
        }

        /// <summary>
        /// Připojí se a začne provádět tasky
        /// </summary>
        public DaemonClient()
        {
            messenger = new Messenger(config.Server);
            try
            {
                if (config.Uuid == null)// Daemon nema Uuid
                    Introduce();

                Login(); // Ziska session id
            }
            catch(BadResponseException e)
            {
                Console.WriteLine("Chyba při pokusu o přihlášení :"+e.Message);
                e.ErrorMessages.ForEach(r => Console.WriteLine(r.id + ":" + r.message + "->" + r.value));
                return;
            }

            if(config.Debug)
                TaskTest();
            else
                ApplyTasks().Wait();
        }

        /// <summary>
        /// Pro testování tasků
        /// </summary>
        private void TaskTest()
        {
            taskHandler.Tasks = messenger.ReadMessage<TaskResponse>("{\"Tasks\":[{\"id\":1,\"uuidDaemon\":\"50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e\",\"name\":\"DebugTask\",\"description\":\"For debugging\",\"taskLocations\":[{\"id\":1,\"source\":{\"id\":6,\"uri\":\"test.com/docs/imgs\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":4,\"host\":\"test.com\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"destination\":{\"id\":7,\"uri\":\"test.com/backups/imgs\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":5,\"host\":\"test.com\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"backupType\":{\"Id\":1,\"ShortName\":\"NORM\",\"LongName\":\"Normal\"},\"times\":[{\"id\":3,\"interval\":0,\"name\":\"Dneska\",\"repeat\":false,\"startTime\":\""+DateTime.Now.AddSeconds(5)+"\",\"endTime\":\"0001-01-01T00:00:00\"},{\"id\":4,\"interval\":"+5+",\"name\":\"Kazdy Patek\",\"repeat\":true,\"startTime\":\"2018-02-23T00:00:00\",\"endTime\":\"0001-01-01T00:00:00\"}]}]}],\"ErrorMessages\":[]}").Tasks ;
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
                config.SessionLength-config.SessionLengthPadding), lastCheck) == -1;
                
        }

        /// <summary>
        /// Zkontroluje jestli je login platný
        /// </summary>
        private void CheckLogin()
        {
            if (config.Session == null || !IsSessionStillValid(config.LastCommunicator/*???jmeno*/))
                Login();
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
            var responseJson = await messenger.SendAsyncGetJson(new TaskMessage() {sessionUuid = config.Session }, "task", HttpMethod.Post);
            var resp = messenger.ReadMessage<TaskResponse>(responseJson);
            if (!messenger.IsSuccessStatusCode()) // TODO: Tohle nemuze byt v async metode pokud neni messenger locknut
                throw new BadResponseException(messenger.StatusCode + " Error",resp.ErrorMessages); //TODO: Custom exception
            return resp.Tasks;
        }

        /// <summary>
        /// Introduces Daemon to the server
        /// </summary>
        public async void Introduce()
        {
            String firstMacAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            IntroductionMessage introductionMessage = new IntroductionMessage()
            {
                preSharedKey = "tRnhF0IfmkDrIZU6dbCusQ==",
                id = 1,
                macAdress = firstMacAddress.ToCharArray(),
                os = Environment.OSVersion.ToString(),
                version = 23
            };

            string response = await messenger.SendAsyncGetJson(introductionMessage, "IntroductionController", System.Net.Http.HttpMethod.Post);
            Console.WriteLine(response);
        }

        /// <summary>
        /// Přihlásí uživatele a zaznamená si session do configu
        /// </summary>
        public void Login()
        {
            
            LoginMessage loginMessage = new LoginMessage() {password = config.Pass,uuid = config.Uuid };
            messenger.Send(loginMessage, "login", HttpMethod.Post);
            LoginResponse response = messenger.ReadMessage<LoginResponse>();
            if (!messenger.IsSuccessStatusCode())
                throw new BadResponseException(messenger.StatusCode + " Error", response.errorMessage);
            config.Session = response.sessionUuid;
        }
    }
}
