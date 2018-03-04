using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminDaemonsController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.Daemons.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        //public ActionResult Logs()
        //{
        //    try
        //    {
        //        using (var db = new Models.MySQLContext())
        //            return View(db.DaemonLogs.ToList());
        //    }
        //    catch (Exception e)
        //    {
        //        //TODO: LOG
        //        return RedirectToAction("Index", "AdminError");
        //    }
        //}

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