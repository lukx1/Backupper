using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminDaemonsController : Controller
    {
        public ActionResult DaemonInfo(int id)
        {
            try
            {
                var daemon = new Models.Admin.DaemonInfoModel();
                daemon.Id = id;
                daemon.Populate();

                return View(daemon);
            }
            catch (Exception e)
            {
                TempData["customMessage"] = e.Message;
                return RedirectToAction("Index", "AdminDaemons");
            }
        }

        public ActionResult Index()
        {
            try
            {
                return View(Models.Admin.DaemonInfoModel.GetAllOnlyBasicInfo());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult Logs()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.DaemonLogs.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult LogedInDaemons()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.LogedInDaemons.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }
    }
}