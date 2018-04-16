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
    public class AdminLocationCredentialsController : AdminBaseController
    {
        public ActionResult Index()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                    return View(db.LocationCredentials.AsQueryable().Include(x => x.LogonType).ToList());
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

                using (var db = new MySQLContext())
                {
                    ViewBag.LogonTypes =
                        db.LogonTypes.Select(x => new SelectListItem()
                            {
                                Value = x.Id.ToString(),
                                Text = x.Name
                            }
                        ).ToArray();
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }

            return View(new Models.LocationCredential());
        }

        [HttpPost]
        public ActionResult NewLocationCredential(Models.LocationCredential cred)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.LocationCredentials.Add(cred);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }

            TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "New credential was successfully created";
            return RedirectToAction("Index", "AdminLocationCredentials");
        }

        [HttpGet]
        public ActionResult EditLocationCredential(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new MySQLContext())
                {
                    ViewBag.LogonTypes =
                        db.LogonTypes.Select(x => new SelectListItem()
                            {
                                Value = x.Id.ToString(),
                                Text = x.Name
                            }
                        ).ToArray();
                }

                using (var db = new Models.MySQLContext())
                {
                    var cred = db.LocationCredentials.Where(x => x.Id == id).Include(x => x.LogonType).FirstOrDefault();
                    if (cred == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Credential does not exists";
                        return RedirectToAction("Index", "AdminLocationCredentials");
                    }

                    ViewBag.LogonTypes =
                        db.LogonTypes.Select(x => new SelectListItem()
                            {
                                Value = x.Id.ToString(),
                                Text = x.Name
                            }
                        ).ToArray();

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
        public ActionResult EditLocationCredential(Models.LocationCredential cred)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.LocationCredentials.AddOrUpdate(cred);
                    db.SaveChanges();
                }

                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Credential was successfully updated";
                return RedirectToAction("Index", "AdminLocationCredentials");
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
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Credential does not exists";
                        return RedirectToAction("Index", "AdminLocationCredentials");
                    }

                    db.LocationCredentials.Remove(dbLocCred);
                    db.SaveChanges();

                    TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Credential was successfully deleted";
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