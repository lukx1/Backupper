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
    /// Slouzi ke komunikaci se serverem
    /// </summary>
    public class DaemonClient
    {
        private Messenger messenger { get; set; }
        private IConfig config = new StaticConfig();
        private TaskHandler taskHandler = new TaskHandler();

        public DaemonClient()
        {
            messenger = new Messenger("http://localhost:57597"); 
            if(config.Uuid == null)
                this.Introduce();
            Login();
            TaskTest();
            //ApplyTasks().Wait();
        }

        private void TaskTest()
        {
            taskHandler.Tasks = messenger.ReadMessage<TaskResponse>("{\"Tasks\":[{\"id\":1,\"uuidDaemon\":\"50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e\",\"name\":\"DebugTask\",\"description\":\"For debugging\",\"taskLocations\":[{\"id\":1,\"source\":{\"id\":6,\"uri\":\"test.com\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":4,\"host\":\"test.com/myName\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"destination\":{\"id\":7,\"uri\":\"test.com\",\"protocol\":{\"Id\":3,\"ShortName\":\"FTP\",\"LongName\":\"File Transfer Protocol\"},\"LocationCredential\":{\"Id\":5,\"host\":\"test.com/myName\",\"port\":21,\"LogonType\":{\"Id\":2,\"Name\":\"Normal\"},\"username\":\"myName\",\"password\":\"abc\"}},\"backupType\":{\"Id\":1,\"ShortName\":\"NORM\",\"LongName\":\"Normal\"},\"times\":[{\"id\":3,\"interval\":0,\"name\":\"Dneska\",\"repeat\":false,\"startTime\":\""+DateTime.Now.AddSeconds(5)+"\",\"endTime\":\"0001-01-01T00:00:00\"},{\"id\":4,\"interval\":"+5+",\"name\":\"Kazdy Patek\",\"repeat\":true,\"startTime\":\"2018-02-23T00:00:00\",\"endTime\":\"0001-01-01T00:00:00\"}]}]}],\"ErrorMessages\":[]}").Tasks ;
            taskHandler.CreateTimers();
        }

        private void CheckLogin()
        {
            if (config.Session == null)
                Login();
            //TODO : else if(> 15 min bez kontaktu)
        }

        private async Task ApplyTasks()
        {
            taskHandler.Tasks = await GetAllTaskFromDB();
            taskHandler.CreateTimers();
        }

        private async Task<List<DbTask>> GetAllTaskFromDB()
        {
            CheckLogin();
            var responseJson = await messenger.SendAsyncGetJson(new TaskMessage() {sessionUuid = config.Session }, "task", HttpMethod.Post);
            if(!messenger.IsSuccessStatusCode())
                throw new HttpRequestException(messenger.StatusCode + " Error"); //TODO: Custom exception
            return messenger.ReadMessage<TaskResponse>(responseJson).Tasks;
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
            if (!messenger.IsSuccessStatusCode())
                throw new HttpRequestException(messenger.StatusCode+" Error");
            LoginResponse response = messenger.ReadMessage<LoginResponse>();
            config.Session = response.sessionUuid;
        }
    }
}
