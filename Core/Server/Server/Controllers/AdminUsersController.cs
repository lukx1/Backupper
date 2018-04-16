using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;
using Shared;

namespace Server.Controllers
{
    [AdminExc]
    public class AdminUsersController : AdminBaseController
    {
        [HttpGet]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult Index()
        {
            using (var db = new Models.MySQLContext())
                return View(db.Users.ToList());
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult UserGroups(int id)
        {
            var model = new Models.Admin.UserGroupsModel(id);
            model.Load();
            return View(model);
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult UserGroups(Models.Admin.UserGroupsModel model)
        {
            model.Save();
            OperationResultMessage = "Group edition was successfull";
            return RedirectToAction("Index", "AdminUsers");
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult NewUser()
        {
            return View(new Models.User());
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult NewUser(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                user.Password = PasswordFactory.HashPasswordPbkdf2(user.Password);

                db.Users.Add(user);
                db.SaveChanges();
            }

            OperationResultMessage = "New user was successfully created";
            return RedirectToAction("Index", "AdminUsers");
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult EditUser(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    OperationResultMessage = "User does not exists";
                    return RedirectToAction("Index", "AdminUsers");
                }
                return View(user);
            }
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult EditUser(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                db.Users.AddOrUpdate(user);
                db.SaveChanges();
            }

            OperationResultMessage = "User was successfully updated";
            return RedirectToAction("Index", "AdminUsers");
        }

        [HttpGet]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult DeleteUser(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbUser = db.Users.FirstOrDefault(x => x.Id == id);
                if (dbUser == null)
                {
                    OperationResultMessage = "User does not exists";
                    return RedirectToAction("Index", "AdminUsers");
                }
                return View(dbUser);
            }
        }

        [HttpPost]
        [AdminSec(Permission.MANAGEOTHERUSERS)]
        public ActionResult DeleteUser(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbUser = db.Users.FirstOrDefault(x => x.Id == user.Id);
                if (dbUser == null)
                {
                    OperationResultMessage = "User does not exists";
                    return RedirectToAction("Index", "AdminUsers");
                }

                db.Users.Remove(dbUser);
                db.SaveChanges();

                OperationResultMessage = "User was successfully deleted";
                return RedirectToAction("Index", "AdminUsers");
            }
        }
    }
}