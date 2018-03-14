using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminUsersController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.Users.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult UserGroups()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.UserGroups.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult Logs()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.UserLogs.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult LogedInUsers()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.LogedInUsers.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

		[HttpGet]
		public ActionResult Delete(int id)
		{
			try
			{
				using (var db = new Models.MySQLContext())
					return View(db.Users.Where(x => x.Id == id).FirstOrDefault());
			}
			catch (Exception e)
			{
				//TODO: LOG
				return RedirectToAction("Index", "AdminError");
			}
		}

		[HttpPost]
		public ActionResult Delete(Models.User model)
		{
			try
			{
				using (var db = new Models.MySQLContext())
				{
					TempData["customMessage"] = "User deleted is OK";
					return RedirectToAction("Index", "AdminUsers");
				}
			}
			catch (Exception e)
			{
				//TODO: LOG
				return RedirectToAction("Index", "AdminError");
			}
		}
	}
}