using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;

namespace Server.Controllers
{
	[AdminExc]
    [AdminSec(Permission.MANAGESERVERSTATUS)]
    public class AdminUptimeController : AdminBaseController
    {
		[AdminSec]
        public ActionResult Index()
        {
            return View();
        }
    }
}