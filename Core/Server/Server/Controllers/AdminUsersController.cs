using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Server.Authentication;
using Shared;

namespace Server.Controllers
{
    [AdminExc]
    [AdminSec(Permission.MANAGEOTHERUSERS, Permission.MANAGESELFUSER)]
    public class AdminUsersController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new Models.MySQLContext())
                return View(db.Users.ToList());
        }

        [HttpGet]
        public ActionResult UserGroups(int id)
        {
            var model = new Models.Admin.UserGroupsModel(id);
            model.Load();
            return View(model);
        }

        [HttpPost]
        public ActionResult UserGroups(Models.Admin.UserGroupsModel model)
        {
            model.Save();
            OperationResultMessage = "Group edition was successfull";
            return RedirectToAction("Index", "AdminUsers");
        }

        [HttpGet]
        public ActionResult NewUser()
        {
            return View(new Models.User());
        }

        [HttpPost]
        public ActionResult NewUser(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                string RSAPair = PasswordFactory.CreateRSAPrivateKey();
                user.PrivateKey = PasswordFactory.EncryptAES(RSAPair, user.Password);
                user.Password = PasswordFactory.HashPasswordPbkdf2(user.Password);
                user.PublicKey = PasswordFactory.GetPublicFromRSAKeyPair(RSAPair);
                db.Users.Add(user);
                db.SaveChanges();
            }

            OperationResultMessage = "New user was successfully created";
            return RedirectToAction("Index", "AdminUsers");
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == id);
                user.Password = null;
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult EditUser(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbUser = db.Users.FirstOrDefault(x => x.Id == user.Id);

                dbUser.Nickname = user.Nickname;
                dbUser.FullName = user.FullName;
                if (!user.Password.IsNullOrWhiteSpace())
                {
                    string RSAPair = PasswordFactory.CreateRSAPrivateKey();
                    dbUser.PrivateKey = PasswordFactory.EncryptAES(RSAPair, user.Password);
                    dbUser.Password = PasswordFactory.HashPasswordPbkdf2(user.Password);
                    dbUser.PublicKey = PasswordFactory.GetPublicFromRSAKeyPair(RSAPair);
                }

                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
            }

            OperationResultMessage = "User was successfully updated";
            return RedirectToAction("Index", "AdminUsers");
        }

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbUser = db.Users.FirstOrDefault(x => x.Id == id);
                return View(dbUser);
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbUser = db.Users.FirstOrDefault(x => x.Id == user.Id);

                db.Users.Remove(dbUser);
                db.SaveChanges();

                OperationResultMessage = "User was successfully deleted";
                return RedirectToAction("Index", "AdminUsers");
            }
        }
    }
}