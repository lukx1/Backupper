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
    [AdminExc]
    [AdminSec(Authentication.Permission.MANAGEPRESHARED)]
    public class AdminPresharedKeysController : AdminBaseController
    {
        public ActionResult Index()
        {
            using (var db = new Models.MySQLContext())
            {
                var model = db.DaemonPreSharedKeys.AsQueryable().Include(x => x.User).ToArray();
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult NewPresharedKey()
        {
            return View(new Models.DaemonPreSharedKey());
        }

        [HttpPost]
        public ActionResult NewPresharedKey(Models.DaemonPreSharedKey key)
        {
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

        [HttpGet]
        public ActionResult DeletePresharedKey(int id)
        {
            using (var db = new MySQLContext())
            {
                var key = db.DaemonPreSharedKeys.AsQueryable().Include(x => x.User).FirstOrDefault(x => x.Id == id);
                return View(key);
            }
        }

        [HttpPost]
        public ActionResult DeletePresharedKey(Models.DaemonPreSharedKey key)
        {
            using (var db = new MySQLContext())
            {
                db.Entry(key).State = EntityState.Deleted;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "AdminPresharedKeys");
        }
    }
}