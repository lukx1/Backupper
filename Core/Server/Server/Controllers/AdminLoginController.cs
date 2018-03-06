using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shared;

namespace Server.Controllers
{
    public class AdminLoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            if (Util.IsUserAlreadyLoggedIn(Session))
                return RedirectToAction("Index", "Admin", null);
            return View(new Models.Admin.LoginModel());
        }


        [HttpPost]
        public ActionResult Login(Models.Admin.LoginModel loginModel)
        {
            try
            {
                Guid guid;
                using (var db = new Models.MySQLContext())
                {
                    var auth = new Authentication.Authenticator(db);
                    guid = auth.LoginUser(loginModel.Nickname, loginModel.Password);
                }

                Session["sessionUuid"] = guid;
                return RedirectToAction("Index", "Admin", null);
            }
            catch (Exception e)
            {
                if (e is NullReferenceException || e is ArgumentException)
                    return View(new Models.Admin.LoginModel());
                else
                    return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "AdminLogin");
        }
    }
}