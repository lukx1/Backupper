using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminPresharedKeysController : Controller
    {
        // GET: AdminPresharedKeys
        public ActionResult Index()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.DaemonPreSharedKeys.ToList());
            }
            catch(Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }
    }
}