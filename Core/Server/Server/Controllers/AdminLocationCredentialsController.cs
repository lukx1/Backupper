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
    public class AdminLocationCredentialsController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                    return View(db.LocationCredentials.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult NewLocationCredential()
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
        public ActionResult NewLocationCredential(Models.Time time)
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
        public ActionResult EditLocationCredential(int id)
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
        public ActionResult EditLocationCredential(Models.Time time)
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
        public ActionResult DeleteLocationCredential(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var cred = db.LocationCredentials.Where(x => x.Id == id).Include(x => x.LogonType).FirstOrDefault();
                    if (cred == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Credentials does not exists";
                        return RedirectToAction("Index", "AdminLocationCredentials");
                    }
                    return View(cred);
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
        public ActionResult DeleteLocationCredential(Models.LocationCredential cred)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var dbLocCred = db.LocationCredentials.FirstOrDefault(x => x.Id == cred.Id);
                    if (dbLocCred == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Credentials does not exists";
                        return RedirectToAction("Index", "AdminLocationCredentials");
                    }

                    db.LocationCredentials.Remove(dbLocCred);
                    db.SaveChanges();

                    TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Credentials was successfully deleted";
                    return RedirectToAction("Index", "AdminLocationCredentials");
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