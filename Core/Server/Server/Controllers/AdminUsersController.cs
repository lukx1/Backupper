using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminUsersController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.Users.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult UserGroups()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.UserGroups.ToList());
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
                    return View(db.UserLogs.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult LogedInUsers()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.LogedInUsers.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }
    }
}