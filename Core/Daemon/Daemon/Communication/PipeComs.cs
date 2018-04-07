using Daemon.Logging;
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
        private Task listener;
        private ILogger logger = LoggerFactory.CreateAppropriate();

        public async Task SendMessageAsync(PipeMessage message)
        {
            using (NamedPipeClientStream client = new NamedPipeClientStream(".", PipeMessage.PIPE_NAME, PipeDirection.InOut, PipeOptions.WriteThrough, System.Security.Principal.TokenImpersonationLevel.Anonymous, HandleInheritability.None))
            {
                await client.ConnectAsync(5000);
                using (var writer = new StreamWriter(client))
                {
                    writer.AutoFlush = true;
                    await client.WriteAsync(message.ToSendable(), 0, PipeMessage.MAX_SIZE_IN_BYTES);
                    client.WaitForPipeDrain();
                }
            }
        }

        public delegate void MessageReceivedDeleg(PipeMessage msg);
        public delegate void MessageReadingFailedDeleg(byte[] bytes);

        public event MessageReceivedDeleg MessageReceived;
        public event MessageReadingFailedDeleg ReadingFailed;

        public Task StartListeningAsync()
        {
            Thread.CurrentThread.Name = "NamedPipe client listener";
            while (true)
            {
                using (NamedPipeClientStream client = new NamedPipeClientStream(".", PipeMessage.PIPE_NAME, PipeDirection.In))
                {
                    client.Connect();
                    //client.ReadTimeout = -1;
                    //client.ReadMode = PipeTransmissionMode.Message;
                    try
                    {
                        
                        byte[] buff = new byte[PipeMessage.MAX_SIZE_IN_BYTES];
                        var res = client.Read(buff, 0, buff.Length);
                        MessageReceived(PipeMessage.Read(buff));
                    }
                    catch (Exception e)
                    {
                        logger.Log($"NamedPipe - Chyba při čtění z pipe{Util.Newline}{e}-{e.Message}{Util.Newline}{e.StackTrace}", LogType.ERROR);
                    }

                }
            }
        }

        public async Task<PipeMessage> ReadMessageAsync()
        {
            using (NamedPipeClientStream client = new NamedPipeClientStream(PipeMessage.PIPE_NAME))
            {
                if (!client.IsConnected)
                    await client.ConnectAsync();
                byte[] buff = new byte[PipeMessage.MAX_SIZE_IN_BYTES];
                var res = await client.ReadAsync(buff, 0, buff.Length);
                return PipeMessage.Read(buff);
            }
        }

        public void Dispose()
        {
            if (listener != null)
                listener.Dispose();
        }
    }
}
