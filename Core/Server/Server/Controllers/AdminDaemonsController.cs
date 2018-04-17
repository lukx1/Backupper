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