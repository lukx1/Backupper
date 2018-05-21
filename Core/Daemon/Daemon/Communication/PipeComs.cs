using Daemon.Logging;
using DaemonShared;
using DaemonShared.Pipes;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daemon
{
    /// <summary>
    /// Komunikace pře pipy s DS
    /// </summary>
    public class PipeComs : IDisposable
    {
        private ILogger logger = LoggerFactory.CreateAppropriate();
        private static Task ListenTask;
        public static bool IsListening { get; private set; } = true;

        /// <summary>
        /// Odešle zprávnu DS
        /// </summary>
        /// <param name="message">Zpráva</param>
        /// <returns>Task</returns>
        public async Task SendMessageAsync(PipeMessage message)
        {
            using (NamedPipeClientStream client = new NamedPipeClientStream(".", PipeMessage.PIPE_NAME, PipeDirection.InOut, PipeOptions.WriteThrough, System.Security.Principal.TokenImpersonationLevel.Anonymous, HandleInheritability.None))
            {
                await client.ConnectAsync(new LoginSettings().PipeConnectTimeoutMs);
                using (var writer = new StreamWriter(client))
                {
                    writer.AutoFlush = true;
                    var bytes = message.ToSendable();
                    await client.WriteAsync(bytes, 0, PipeMessage.MAX_SIZE_IN_BYTES);
                    logger.Log("NamedPipe - Odeslána zpráva " + message.Code, LogType.DEBUG);
                    client.WaitForPipeDrain();
                }
            }
        }
        public delegate void MessageReceivedDeleg(PipeMessage msg);
        public delegate void MessageReadingFailedDeleg(Exception e,byte[] bytes);

        /// <summary>
        /// Zpráva přijata
        /// </summary>
        public static event MessageReceivedDeleg MessageReceived;
        /// <summary>
        /// Zpráva přijata, ale čtení jí selhalo
        /// </summary>
        public static event MessageReadingFailedDeleg ReadingFailed;

        private Task PipeThread()
        {
            return Task.Run(() =>
            {
                using (NamedPipeServerStream serverStream = new NamedPipeServerStream(PipeMessage.PIPE_NAME, PipeDirection.InOut, 2, PipeTransmissionMode.Message))
                {
                    serverStream.WaitForConnection();
                    //client.ReadTimeout = -1;
                    byte[] buff = null;
                    //client.ReadMode = PipeTransmissionMode.Message;
                    try
                    {

                        buff = new byte[PipeMessage.MAX_SIZE_IN_BYTES];
                        var res = serverStream.Read(buff, 0, buff.Length);
                        MessageReceived(PipeMessage.Read(buff));
                    }
                    catch (Exception e)
                    {
                        ReadingFailed(e, buff);
                        logger.Log($"NamedPipe - Chyba při čtění z pipe{Environment.NewLine}{e}-{e.Message}{Environment.NewLine}{e.StackTrace}", LogType.ERROR);
                    }

                }
            });
        }

        /// <summary>
        /// Ukončí pipy a komunikaci s DS
        /// </summary>
        public static void StopListening()
        {
            IsListening = false;
        }

        /// <summary>
        /// Zapne komunikaci s DS. Freezuje thread
        /// </summary>
        public void StartListening()
        {
            IsListening = true;
            if (ListenTask == null)
                ListenTask = HiddenListenAsync();
        }

        private async Task HiddenListenAsync()
        {
            while (IsListening)
            {
                var thread = PipeThread();
                await thread;
            }
        }

        /// <summary>
        /// Počká dokud nepřijde zpráva od DS a poté jí přečte.
        /// Pokud již nějaká čeká tak je přečtena ihned.
        /// Freezuje
        /// </summary>
        /// <returns>Zpráva</returns>
        public async Task<PipeMessage> ReadMessageAsync()
        {
            using (NamedPipeClientStream client = new NamedPipeClientStream(PipeMessage.PIPE_NAME))
            {
                await client.ConnectAsync();
                byte[] buff = new byte[PipeMessage.MAX_SIZE_IN_BYTES];
                var res = await client.ReadAsync(buff, 0, buff.Length);
                return PipeMessage.Read(buff);
            }
        }

        /// <summary>
        /// Zničí PipeComs a jeho thready
        /// </summary>
        public void Dispose()
        {
            StopListening();
            ListenTask.Dispose();
        }
    }
}
