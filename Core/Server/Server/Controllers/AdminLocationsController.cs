using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;
using Server.Models;
using Shared;

namespace Server.Controllers
{
    public class AdminLocationsController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                    return View(db.Times.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult NewTime()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }

            return View(new Models.Time());
        }

        [HttpPost]
        public ActionResult NewTime(Models.Time time)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.Times.Add(time);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }

            TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "New time was successfully created";
            return RedirectToAction("Index", "AdminTimes");
        }

        [HttpGet]
        public ActionResult EditTime(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var time = db.Times.FirstOrDefault(x => x.Id == id);
                    if (time == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Time does not exists";
                        return RedirectToAction("Index", "AdminTimes");
                    }
                    return View(time);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult EditTime(Models.Time time)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.Times.AddOrUpdate(time);
                    db.SaveChanges();
                }

                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Time was successfully updated";
                return RedirectToAction("Index", "AdminTimes");
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult DeleteTime(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var time = db.Times.FirstOrDefault(x => x.Id == id);
                    if (time == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Time does not exists";
                        return RedirectToAction("Index", "AdminTimes");
                    }
                    return View(time);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult DeleteTime(Models.Time time)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var dbTime = db.Times.FirstOrDefault(x => x.Id == time.Id);
                    if (dbTime == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Time does not exists";
                        return RedirectToAction("Index", "AdminTimes");
                    }

                    db.Times.Remove(dbTime);
                    db.SaveChanges();

                    TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Time was successfully deleted";
                    return RedirectToAction("Index", "AdminUsers");
                }
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