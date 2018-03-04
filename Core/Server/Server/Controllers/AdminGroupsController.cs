using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminGroupsController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.Groups.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }
    }
}