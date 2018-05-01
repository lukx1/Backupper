using Daemon.Communication;
using Daemon.Logging;
using DaemonShared;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daemon
{
    /// <summary>
    /// Opakovaně kontaktuje server aby session nevypršel
    /// </summary>
    public class SessionRefresher : IDisposable
    {
        private Task ticker;
        private LoginSettings settings = new LoginSettings();
        private Authenticator logginer;
        private DateTime refreshAt;
        private DateTime wakeUpAt;
        private bool externRec = false;
        private ILogger logger = ConsoleLogger.CreateInstance();

        public SessionRefresher(Authenticator logginer)
        {
            this.logginer = logginer;
        }

        /// <summary>
        /// Spustí tickování sezení
        /// </summary>
        public void Run()
        {
            ticker = Task.Run(() => LoginTicker());
        }

        public void ExternalyRefreshed()
        {
            logger.Log("Externí refresh přijmut", LogType.DEBUG);
            refreshAt = DateTime.Now.AddMinutes((settings.SessionLengthMinutes - settings.SessionLengthPaddingMinutes));
            externRec = true;
        }

        private int CalculateSleepTimeMs()
        {
            if (externRec)
            {
                externRec = false;
                int sleeptime = (int)(refreshAt - wakeUpAt).TotalMilliseconds;
                logger.Log($"Bude proveden ext. refresh za {sleeptime/1000/60}min",LogType.DEBUG);
                return sleeptime;
            }
            else
                return (settings.SessionLengthMinutes - settings.SessionLengthPaddingMinutes) * 60000/*min do ms*/;
        }

        private async Task LoginTicker()
        {
            Thread.CurrentThread.Name = "LoginTicker";
            while (true)
            {
                int sleepTimeMs = CalculateSleepTimeMs();
                wakeUpAt = DateTime.Now.AddMilliseconds(sleepTimeMs);
                Thread.Sleep(sleepTimeMs);
                await logginer.AttemptLogin();
                if (settings.LoggingLevel >= (int)LogType.DEBUG)
                    Console.WriteLine($"{DateTime.Now} Login refreshnut");
            }
        }

        public void Dispose()
        {
            this.ticker.Dispose();
        }
    }
}
