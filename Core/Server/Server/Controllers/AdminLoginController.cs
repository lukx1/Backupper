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
                var guid = Authentication.StaticUserHelper.LoginUser(loginModel.Nickname, loginModel.Password);
                Session["sessionUuid"] = guid;
                return RedirectToAction("Index", "Admin", null);
            }
            catch (Exception e)
            {
                if (e is Exceptions.NonExistingUserException || e is Exceptions.NotMatchingPasswordException)
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