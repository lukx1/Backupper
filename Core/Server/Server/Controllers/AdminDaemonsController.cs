using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;

namespace Server.Controllers
{
	[AdminExc]
    [AdminSec(Permission.MANAGEOTHERDAEMONS, Permission.MANAGESELFDAEMONS)]
    public class AdminDaemonsController : AdminBaseController
    {
        [HttpGet]
        public ActionResult UnacceptedDaemons()
        {
            using (var db = new Models.MySQLContext())
            {
                var daemons = db.WaitingForOneClicks
                    .AsQueryable()
                    .Include(x => x.DaemonInfo)
                    .Where(x => x.Confirmed == false)
                    .ToArray();
                return View(daemons);
            }
        }

        [HttpPost]
        public JsonResult AcceptDaemon(int id)
        {
            JsonResult result = new JsonResult();

            using (var db = new Models.MySQLContext())
            {
                var wfoc = db.WaitingForOneClicks
                    .FirstOrDefault(x => x.Id == id);
                if(wfoc != null)
                {
                    wfoc.Confirmed = true;
                    db.SaveChanges();
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    result.Data = new { success = true };
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                    result.Data = new { success = false };
                }
            }

            return result;
        }

        [HttpPost]
        public JsonResult DeclineDaemon(int id)
        {
            JsonResult result = new JsonResult();

            using (var db = new Models.MySQLContext())
            {
                var wfoc = db.WaitingForOneClicks
                    .FirstOrDefault(x => x.Id == id);
                if (wfoc != null)
                {
                    db.WaitingForOneClicks.Remove(wfoc);
                    db.SaveChanges();
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    result.Data = new { success = true };
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                    result.Data = new { success = false };
                }
            }

            return Json(result);
        }

        [HttpGet]
        public ActionResult Index()
        {
			using (var db = new Models.MySQLContext())
			{
			    var daemons = db.Daemons
			        .AsQueryable()
			        .Include(x => x.DaemonInfo)
			        .Include(x => x.User)
			        .ToArray();
			    return View(daemons);
			}
        }

        [HttpGet]
        public ActionResult DaemonGroups(int id)
        {
			var model = new Models.Admin.DaemonGroupsModel(id);
			model.Load();
			return View(model);
        }

        [HttpPost]
        public ActionResult DaemonGroups(Models.Admin.DaemonGroupsModel model)
        {
			model.Save();
			TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Group edition was successfull";
			return RedirectToAction("Index", "AdminDaemons");
        }

		[HttpGet]
        public ActionResult DaemonsByUser(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var daemons = db.Daemons
                    .Where(x => x.IdUser == id)
                    .Include(x => x.DaemonInfo)
                    .Include(x => x.User)
                    .ToArray();
                return View("Index", daemons);
            }
        }
    }
}