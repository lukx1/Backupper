using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;

namespace Server.Controllers
{
    [AdminExc]
    public class AdminGroupsController : AdminBaseController
    {
        [HttpGet]
        [AdminSec]
        public ActionResult Index()
        {
            using (var db = new Models.MySQLContext())
                return View(db.Groups.ToList());
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEPERMISSION)]
        public ActionResult GroupPermissions(int id)
        {
            var model = new Models.Admin.GroupPermissionsModel(id);
            model.Load();
            return View(model);
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEPERMISSION)]
        public ActionResult GroupPermissions(Models.Admin.GroupPermissionsModel model)
        {
            model.Save();
            OperationResultMessage = "Permission edition was successfull";
            return RedirectToAction("Index", "AdminGroups");
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEGROUPS)]
        public ActionResult NewGroup()
        {
            return View(new Models.Group());
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEGROUPS)]
        public ActionResult NewGroup(Models.Group group)
        {
            using (var db = new Models.MySQLContext())
            {
                db.Groups.Add(group);
                db.SaveChanges();
            }

            OperationResultMessage = "New group was successfully created";
            return RedirectToAction("Index", "AdminGroups");
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEGROUPS)]
        public ActionResult EditGroup(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var group = db.Groups.FirstOrDefault(x => x.Id == id);
                return View(group);
            }
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEGROUPS)]
        public ActionResult EditGroup(Models.Group group)
        {
            using (var db = new Models.MySQLContext())
            {
                db.Groups.AddOrUpdate(group);
                db.SaveChanges();
            }

            OperationResultMessage = "Group was successfully updated";
            return RedirectToAction("Index", "AdminGroups");
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEGROUPS)]
        public ActionResult DeleteGroup(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var group = db.Groups.FirstOrDefault(x => x.Id == id);
                return View(group);
            }
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEGROUPS)]
        public ActionResult DeleteGroup(Models.Group group)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbUser = db.Groups.FirstOrDefault(x => x.Id == group.Id);
                db.Groups.Remove(dbUser);
                db.SaveChanges();

                OperationResultMessage = "Group was successfully deleted";
                return RedirectToAction("Index", "AdminGroups");
            }
        }
    }
}