using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Configuration;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (string.IsNullOrWhiteSpace(Objects.ConnectionStringHelper.ConnectionString))
            {
                return RedirectToAction("Index", "AdminSetup");
            }

            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
