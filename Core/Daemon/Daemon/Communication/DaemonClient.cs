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
        private IConfig config = new TempConfig();
        private TaskHandler taskHandler = new TaskHandler();

        public DaemonClient()
        {
            messenger = new Messenger("http://localhost:3393/"); 
            if(config.GetUuid() == null)
                this.Introduce();
            Login();
            ApplyTasks();
        }



        private void CheckLogin()
        {
            if (config.GetSession() == null)
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
            var responseJson = await messenger.SendAsyncGetJson(new TaskMessage(), "task", HttpMethod.Post);
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
            LoginMessage loginMessage = new LoginMessage() {password = config.GetPass(),uuid = config.GetUuid() };
            messenger.Send(loginMessage, "login", HttpMethod.Post);
            if (!messenger.IsSuccessStatusCode())
                throw new HttpRequestException(messenger.StatusCode+" Error");
            LoginResponse response = messenger.ReadMessage<LoginResponse>();
            config.SetSession(response.sessionUuid);
        }
    }
}
