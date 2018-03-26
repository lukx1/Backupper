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
                    //var auth = new Authentication.Authenticator(db);
                    //if (!auth.IsUserAllowed((Guid)Session[Objects.MagicStrings.SESSION_UUID],
                    //    Permission.MANAGEPRESHARED))
                    //{
                    //    TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "You are not allowed here";
                    //    return RedirectToAction("Login", "AdminLogin");
                    //}

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
                    key.PreSharedKey = PasswordFactory.CreateRandomPassword(16);

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
    }
}