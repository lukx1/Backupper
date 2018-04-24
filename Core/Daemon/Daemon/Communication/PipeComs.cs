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
    public class PipeComs : IDisposable
    {
        private ILogger logger = LoggerFactory.CreateAppropriate();
        private static Task ListenTask;
        public static bool IsListening { get; private set; } = true;

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

        public static event MessageReceivedDeleg MessageReceived;
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

        public static void StopListening()
        {
            IsListening = false;
        }

        /// <summary>
        /// Freezuje
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

        public void Dispose()
        {
            StopListening();
            ListenTask.Dispose();
        }
    }
}
