using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shared;

namespace Server.Controllers
{
    [AdminExc]
    public class AdminLoginController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            if (Util.IsUserAlreadyLoggedIn(Session))
                return RedirectToAction("Index", "Admin");
            return View(new Models.Admin.LoginModel());
        }


        [HttpPost]
        public ActionResult Login(Models.Admin.LoginModel loginModel)
        {
            Guid guid;
            using (var db = new Models.MySQLContext())
            {
                var auth = new Authentication.Authenticator(db);
                guid = auth.LoginUser(loginModel.Nickname, loginModel.Password);
            }

            SessionUuid = guid;
            return RedirectToAction("Index", "Admin");
        }

        [AdminSec(Authentication.Permission.LOGIN)]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "AdminLogin");
        }
    }
}