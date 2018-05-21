using Daemon.Logging;
using DaemonShared;
using Shared;
using Shared.ToDaemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    /// <summary>
    /// Nepoužívat
    /// </summary>
    [Obsolete]
    public static class ToServerListener
    {

        private static IEnumerable<Type> Controllers;
        private static TcpListener server;
        private static LoginSettings settings = new LoginSettings();
        private static ILogger logger = LoggerFactory.CreateAppropriate();
        public static volatile bool Run = false;


        private static void LoadControllers()
        {
            Controllers = typeof(ListenController)
                        .Assembly.GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(ListenController)) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null && t.Name.ToUpper().EndsWith("CONTROLLER"))
                        .Select(t => t);
        }

        public static void StartServer()
        {
            if (server != null)
                return;
            server = new TcpListener(System.Net.IPAddress.Parse(settings.ReceiveIP),settings.ReceivePort);
            logger.Log($"Vytvoren in-server pro TCP {settings.ReceiveIP}:{settings.ReceivePort}",Shared.LogType.DEBUG);
            Task.Run(() => StartListening());
        }

        private static int ReadByteSize(NetworkStream stream)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4)
                throw new InvalidOperationException("Nebylo prijato dost bytu");
            return BitConverter.ToInt32(buffer, 0);
        }

        private static TDMessageCode GetCode(NetworkStream stream)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4)
                throw new InvalidOperationException("Nebylo prijato dost bytu");
            return (TDMessageCode)BitConverter.ToInt32(buffer, 0);
        }

        private static void HandleSocket(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                TSLDecoder decoder = new TSLDecoder();
                var buff = decoder.Decode(stream);
                SendToController(buff, decoder.Code, stream);
            }
            finally
            {
                client.Close();
            }
        }

        private static void SendToController(byte[] msg, TDMessageCode code, NetworkStream stream)
        {
            foreach (var controller in Controllers)
            {
                if (controller.Name.ToUpper().StartsWith(code.ToString().ToUpper()))
                {
                    ListenController instance = (ListenController)Activator.CreateInstance(controller);
                    var recMsg = instance.Receive(Encoding.UTF8.GetString(msg));
                    TDMessage tdm = TDMessage.FromObject(new TDMessage.Message() { Code = recMsg.Code,SerializeToContent = recMsg});
                    var bytes = tdm.ContentsAsArray;
                    stream.Write(bytes,0,bytes.Length);
                }
            }
        }


        private static void StartListening()
        {
            Run = true;
            server.Start();
            while (Run)
            {
                var client = server.AcceptTcpClient();
                Task.Run(() => HandleSocket(client));
            }
            server.Stop();
        }

        static ToServerListener()
        {
            LoadControllers();
        }

    }
}
