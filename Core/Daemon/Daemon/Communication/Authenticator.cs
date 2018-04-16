using Daemon.Logging;
using DaemonShared;
using Shared;
using Shared.LogObjects;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Communication
{
    public class Authenticator
    {
        private Messenger messenger;
        private LoginSettings settings = new LoginSettings();
        private ILogger logger;

        public Authenticator(Messenger messenger)
        {
            this.messenger = messenger;
            logger = ConsoleLogger.CreateInstance();
        }

        /// <summary>
        /// Přihlásí se, pokud se vyskytne chyba hodí InetException<LoginResponse>
        /// </summary>
        /// <returns>Guid přijaté od serveru</returns>
        public async Task<Guid> Login()
        {
            LoginMessage loginMessage = new LoginMessage() { password = settings.Password, uuid = settings.Uuid };
            var resp = await messenger.SendAsync<LoginResponse>(loginMessage, "login", HttpMethod.Post);
            return resp.ServerResponse.sessionUuid;
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
            logger.Log("Začíná introducování", LogType.DEBUG);

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
            logger.Log("Introduction úspěšný", LogType.INFORMATION);
            settings.Uuid = resp.ServerResponse.uuid;
            settings.Password = resp.ServerResponse.password;
        }

        /// <summary>
        /// Pokusí se přihlásit a uložit sessionUuid
        /// </summary>
        /// <returns>True pokud úspěšné, false jinak</returns>
        public async Task<Guid> AttemptLogin()
        {
            try
            {
                Guid resp = await Login(); // Ziska session id
                return resp;
            }
            catch (INetException<LoginResponse> e)
            {
                logger.Log("Chyba při pokusu o přihlášení :" + e.Message,LogType.ERROR);
                e.ErrorMessages.ForEach(r => logger.Log(r.id + ":" + r.message + "->" + r.value,LogType.ERROR));
                var x = Task.Run(async () => await logger.ServerLogAsync(
                    new DaemonFailedLoginLog(
                        LogType.ERROR,
                        settings.SSLUse ? settings.SSLServer : settings.Server,
                        settings.Uuid,
                        settings.Password == null,
                        e.ErrorMessages
                    )));
                return Guid.Empty;
            }

        }

    }
}
