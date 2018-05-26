using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shared;
using Server.Objects.AdminExceptions;
using System.Web.Routing;

namespace Server.Controllers
{
    
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
            Guid guid = default(Guid);
            using (var db = new Models.MySQLContext())
            {
                var auth = new Authentication.Authenticator(db);
                try
                {
                    guid = auth.LoginUser(loginModel.Nickname, loginModel.Password);
                }
                catch(AdminSecurityException ex)
                {
                    var res = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "AdminLogin",
                            action = "Login"
                        }
                    ));

                    ErrorMessage = ex.Message;
                    return RedirectToAction("Login", "AdminLogin");
                }
                catch(Exception ex)
                {
                    throw ex;
                }

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