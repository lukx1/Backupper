using Server.Models;
using Server.Objects;
using Shared.LogObjects;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminPowerController : Controller
    {

        private void StartHiddenCmdProcess(string args)
        {
            ProcessStartInfo proc = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd",
                Arguments = args
            };
            Process.Start(proc);
        }

        private void SubmitLog(ServerStatusLog.ServerStatusInfo.Status status)
        {
            using (MySQLContext sql = new MySQLContext())
            {
                SqlLogger logger = new SqlLogger();
                logger.SubmitLogAsync(new ServerStatusLog(status)).Wait();
            }
        }

        public ActionResult Exit()
        {
            if (!Util.IsUserAlreadyLoggedIn(Session))
                return RedirectToAction("Login", "AdminLogin", null);
            SubmitLog(ServerStatusLog.ServerStatusInfo.Status.EXITING);
            Environment.Exit(0);
            return null;
        }

        public ActionResult Shutdown()
        {
            if (!Util.IsUserAlreadyLoggedIn(Session))
                return RedirectToAction("Login", "AdminLogin", null);
            SubmitLog(ServerStatusLog.ServerStatusInfo.Status.SHUTTING_DOWN);
            StartHiddenCmdProcess("/C shutdown -t 0");
            return null;
        }

        public ActionResult Restart()
        {
            if (!Util.IsUserAlreadyLoggedIn(Session))
                return RedirectToAction("Login", "AdminLogin", null);
            SubmitLog(ServerStatusLog.ServerStatusInfo.Status.RESTARTING);
            StartHiddenCmdProcess("/C shutdown -r -t 0");
            return null;
        }
    }
}