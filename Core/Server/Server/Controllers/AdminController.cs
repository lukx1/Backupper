using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shared;
using Server.Authentication;
using System.Web.Configuration;

namespace Server.Controllers
{

    [AdminSec]
    public class AdminController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if(string.IsNullOrWhiteSpace(Objects.ConnectionStringHelper.ConnectionString))
            {
                return RedirectToAction("Index", "AdminSetup");
            }

            return View();
        }
    }
}