using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{
    public class ConsoleLogger : ILogger
    {
        private LoginSettings settings = new LoginSettings();

        private void SetColor(LogType logType)
        {
            switch (logType)
            {
                case LogType.EMERGENCY:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogType.ALERT:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogType.CRITICAL:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case LogType.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.NOTIFICATION:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case LogType.INFORMATION:
                    break;
                case LogType.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }
        }

        public void Log(string message, LogType logType)
        {
            if ((int)logType > settings.LoggingLevel)
                return;
            SetColor(logType);
            Console.WriteLine($"{DateTime.Now} - {logType.ToString()} - {message}");
            Console.ResetColor();
        } 
    }
}