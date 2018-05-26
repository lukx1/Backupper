using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    
    public class AdminInfoController : AdminBaseController
    {
        public ActionResult PublicInfo()
        {
            var info = new Models.Admin.PublicInfo();
            info.Load();

            return View(info);
        }
    }
}