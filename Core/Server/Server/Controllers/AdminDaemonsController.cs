using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var daemons = db.Daemons
                        .AsQueryable()
                        .Include(x => x.DaemonInfo)
                        .Include(x => x.User)
                        .ToArray();
                    return View(daemons);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult DaemonGroups(int id)
        {
            try
            {
                var model = new Models.Admin.DaemonGroupsModel(id);
                model.Load();
                return View(model);
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult DaemonGroups(Models.Admin.DaemonGroupsModel model)
        {
            try
            {
                model.Save();
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Group edition was successfull";
                return RedirectToAction("Index", "AdminDaemons");
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult DaemonsByUser(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var daemons = db.Daemons
                        .Where(x => x.IdUser == id)
                        .Include(x => x.DaemonInfo)
                        .Include(x => x.User)
                        .ToArray();
                    return View("Index", daemons);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult LogedInDaemons()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                    return View(db.LogedInDaemons.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }
    }
}