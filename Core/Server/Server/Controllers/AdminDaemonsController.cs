using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;

namespace Server.Controllers
{
	
    [AdminSec(Permission.MANAGEOTHERDAEMONS, Permission.MANAGESELFDAEMONS)]
    public class AdminDaemonsController : AdminBaseController
    {
        [HttpGet]
        public ActionResult UnacceptedDaemons()
        {
            using (var db = new Models.MySQLContext())
            {
                ViewBag.WoCList = db.WaitingForOneClicks
                    .AsQueryable()
                    .Include(x => x.DaemonInfo)
                    .Where(x => x.Confirmed == false)
                    .ToArray();
                return View(new Models.Admin.UnacceptedDaemonsModel());
            }
        }

        [HttpPost]
        public ActionResult UnacceptedDaemons(Models.Admin.UnacceptedDaemonsModel model)
        {
            using (var db = new Models.MySQLContext())
            {
                var wfoc = db.WaitingForOneClicks
                    .FirstOrDefault(x => x.Id == model.Id);
                if (wfoc != null)
                {
                    if(model.IsDaemonAccepted)
                    {
                        wfoc.Confirmed = true;
                        wfoc.DaemonInfo.Name = model.Name;
                        OperationResultMessage = "Daemon was accepted";
                    }
                    else
                    {
                        db.WaitingForOneClicks.Remove(wfoc);
                        OperationResultMessage = "Daemon was declined";
                    }
                    db.SaveChanges();

                }
                else
                {
                    ErrorMessage = "No daemon was waiting";
                }
            }

            return View("UnacceptedDaemons");
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
        public ActionResult EditCustomName(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var daemon = db.Daemons.AsQueryable().Include(x => x.DaemonInfo).Include(x => x.User).FirstOrDefault(x => x.Id == id);
                if(daemon == null)
                {
                    ErrorMessage = "Invalid daemon";
                    return RedirectToAction("Index", "AdminDaemons");
                }

                return View(daemon);
            }
        }

        [HttpPost]
        public ActionResult EditCustomName(Models.Daemon daemon)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbDaemon = db.Daemons.FirstOrDefault(x => x.Id == daemon.Id);
                if (dbDaemon == null)
                {
                    ErrorMessage = "Invalid daemon";
                    return RedirectToAction("Index", "AdminDaemons");
                }

                dbDaemon.DaemonInfo.Name = daemon.DaemonInfo.Name;
                db.SaveChanges();

                return RedirectToAction("Index", "AdminDaemons");
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