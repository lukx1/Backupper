using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shared;
using Server.Authentication;

namespace Server.Controllers
{
	
    [AdminSec]
    public class AdminController : AdminBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}