using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shared;
using Server.Authentication;

namespace Server.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if(!Util.IsUserAlreadyLoggedIn(Session))
                return RedirectToAction("Login", "AdminLogin", null);
            return View();
        }
    }
}