using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;
using Server.Models;
using Shared;

namespace Server.Controllers
{
    public class AdminPresharedKeysController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var model = db.DaemonPreSharedKeys.AsQueryable().Include(x => x.User).ToArray();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult NewPresharedKey()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                return View(new Models.DaemonPreSharedKey());
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult NewPresharedKey(Models.DaemonPreSharedKey key)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new MySQLContext())
                {
                    var auth = new Authenticator(db);
                    var user = auth.GetUserFromUuid((Guid)Session[Objects.MagicStrings.SESSION_UUID]);

                    key.IdUser = user.Id;
                    key.PreSharedKey = PasswordFactory.HashPasswordPbkdf2(PasswordFactory.CreateRandomPassword(16));

                    db.DaemonPreSharedKeys.Add(key);

                    db.SaveChanges();
                }

                return RedirectToAction("Index", "AdminPresharedKeys");
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult DeletePresharedKey(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new MySQLContext())
                {
                    var key = db.DaemonPreSharedKeys.AsQueryable().Include(x => x.User).FirstOrDefault(x => x.Id == id);
                    if (key == null)
                        throw new Exception("Preshared key does not exists");
                    return View(key);
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
        public ActionResult DeletePresharedKey(Models.DaemonPreSharedKey key)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new MySQLContext())
                {
                    db.Entry(key).State = EntityState.Deleted;
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "AdminPresharedKeys");
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