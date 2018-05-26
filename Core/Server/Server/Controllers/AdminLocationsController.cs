using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;
using Server.Models;
using Server.Models.Admin;
using Shared;

namespace Server.Controllers
{
    
    [AdminSec(Authentication.Permission.MANAGELOCATIONS)]
    public class AdminLocationsController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new Models.MySQLContext())
                return View(db.Locations.AsQueryable().Include(x => x.Protocol).Include(x => x.LocationCredential).ToList());
        }

        [HttpGet]
        public ActionResult NewLocation()
        {
            return View(new LocationModel());
        }

        [HttpPost]
        public ActionResult NewLocation(Models.Admin.LocationModel loc)
        {
            loc.Create();

            OperationResultMessage = "New location was successfully created";
            return RedirectToAction("Index", "AdminLocations");
        }

        [HttpGet]
        public ActionResult EditLocation(int id)
        {
            return View(new LocationModel(id));
        }

        [HttpPost]
        public ActionResult EditLocation(Models.Admin.LocationModel loc)
        {
            loc.Update();
            OperationResultMessage = "Location was successfully updated";
            return RedirectToAction("Index", "AdminLocations");
        }

        [HttpGet]
        public ActionResult DeleteLocation(int id)
        {
            return View(new LocationModel(id));
        }

        [HttpPost]
        public ActionResult DeleteLocation(Models.Admin.LocationModel loc)
        {
            loc.Delete();
            OperationResultMessage = "Location was successfully deleted";
            return RedirectToAction("Index", "AdminLocations");
        }

        [HttpGet]
        public ActionResult ShowOrAssignCredential(int id)
        {
            return View(new Models.Admin.ShowOrAssignCredentialModel(id));
        }

        [HttpPost]
        public ActionResult ShowOrAssignCredential(Models.Admin.ShowOrAssignCredentialModel model)
        {
            model.Save();
            OperationResultMessage = "Operation completed successfully";
            return RedirectToAction("Index", "AdminLocations");
        }
    }
}