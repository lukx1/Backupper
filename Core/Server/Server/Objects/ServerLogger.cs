using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Server.Objects
{
    public class ServerLogger
    {
        private static readonly ServerLogger _instance = new ServerLogger();

        protected ILog regularLogger;
        protected static ILog debugLogger;

        private ServerLogger()
        {
            regularLogger = LogManager.GetLogger("RegularLogger");
            debugLogger = LogManager.GetLogger("DebugLogger");
        }

        public static void Debug(string message)
        {
            debugLogger.Debug(message);
        }

        public static void Debug(string message, System.Exception exception)
        {
            debugLogger.Debug(message, exception);
        }

        public static void Info(string message)
        {
            _instance.regularLogger.Info(message);
        }

        public static void Info(string message, System.Exception exception)
        {
            _instance.regularLogger.Info(message, exception);
        }

        public static void Warn(string message)
        {
            _instance.regularLogger.Warn(message);
        }

        public static void Warn(string message, System.Exception exception)
        {
            _instance.regularLogger.Warn(message, exception);
        }

        public static void Error(string message)
        {
            _instance.regularLogger.Error(message);
        }

        public static void Error(string message, System.Exception exception)
        {
            _instance.regularLogger.Error(message, exception);
        }

        public static void Fatal(string message)
        {
            _instance.regularLogger.Fatal(message);
        }

        public static void Fatal(string message, System.Exception exception)
        {
            _instance.regularLogger.Fatal(message, exception);
        }
    }
}