using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Core;

namespace Server.Objects
{
    /// <summary>
    /// Slouzi pro lokalni logovani na serveru
    /// </summary>
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

        public static void Emergency(string message)
        {
            Emergency(message, null);
        }

        public static void Emergency(string message, Exception exception)
        {
            _instance.regularLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Emergency, message, exception);
        }

        public static void Alert(string message)
        {
            Alert(message, null);
        }

        public static void Alert(string message, Exception exception)
        {
            _instance.regularLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Alert, message, exception);
        }

        public static void Critical(string message)
        {
            Critical(message, null);
        }

        public static void Critical(string message, Exception exception)
        {
            _instance.regularLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Critical, message, exception);
        }

        public static void Error(string message)
        {
            Error(message, null);
        }

        public static void Error(string message, Exception exception)
        {
            _instance.regularLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Error, message, exception);
        }

        public static void Warning(string message)
        {
            Warning(message, null);
        }

        public static void Warning(string message, Exception exception)
        {
            _instance.regularLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Warn, message, exception);
        }

        public static void Notification(string message)
        {
            Notification(message, null);
        }

        public static void Notification(string message, Exception exception)
        {
            _instance.regularLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Notice, message, exception);
        }

        public static void Information(string message)
        {
            Information(message, null);
        }

        public static void Information(string message, Exception exception)
        {
            _instance.regularLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Info, message, exception);
        }
        public static void Debug(string message)
        {
            Debug(message, null);
        }

        public static void Debug(string message, Exception exception)
        {
            debugLogger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, Level.Debug, message, exception);
        }
    }
}