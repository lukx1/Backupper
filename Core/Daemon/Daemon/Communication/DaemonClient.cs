using System;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.NetMessages;
using System.Net.NetworkInformation;

namespace Daemon.Communication
{
    /// <summary>
    /// Slouzi ke komunikaci se serverem
    /// </summary>
    public class DaemonClient
    {
        Messenger messenger { get; set; }

        public DaemonClient()
        {
            messenger = new Messenger("http://localhost:3393/"); 
            this.Introduce();
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
        /// Loggins Daemon to the server
        /// </summary>
        public void Login()
        {
            LoginMessage loginMessage = new LoginMessage();
        }
    }
}
