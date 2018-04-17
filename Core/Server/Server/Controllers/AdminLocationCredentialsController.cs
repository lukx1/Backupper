using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Server.Authentication;
using Shared;

namespace Server.Controllers
{
    [AdminExc]
    [AdminSec(Permission.MANAGECREDENTIALS)]
    public class AdminLocationCredentialsController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new Models.MySQLContext())
                return View(db.LocationCredentials.AsQueryable().Include(x => x.LogonType).ToList());
        }

        [HttpGet]
        public ActionResult NewLocationCredential()
        {
            using (var db = new Models.MySQLContext())
            {
                ViewBag.LogonTypes =
                    db.LogonTypes.Select(x => new SelectListItem()
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }
                    ).ToArray();
            }

            return View(new Models.LocationCredential());
        }

        [HttpPost]
        public ActionResult NewLocationCredential(Models.LocationCredential cred)
        {
            using (var db = new Models.MySQLContext())
            {
                db.LocationCredentials.Add(cred);
                db.SaveChanges();
            }

            OperationResultMessage = "New credential was successfully created";
            return RedirectToAction("Index", "AdminLocationCredentials");
        }

        [HttpGet]
        public ActionResult EditLocationCredential(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var cred = db.LocationCredentials.Where(x => x.Id == id).Include(x => x.LogonType).FirstOrDefault();

                ViewBag.LogonTypes =
                    db.LogonTypes.Select(x => new SelectListItem()
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }
                    ).ToArray();

                cred.Password = null;

                return View(cred);
            }
        }

        [HttpPost]
        public ActionResult EditLocationCredential(Models.LocationCredential cred)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbLocCred = db.LocationCredentials.FirstOrDefault(x => x.Id == cred.Id);

                dbLocCred.Username = cred.Username;
                if(!cred.Password.IsNullOrWhiteSpace())
                    dbLocCred.Password = cred.Password;
                dbLocCred.Host = cred.Host;
                dbLocCred.Port = cred.Port;
                dbLocCred.IdLogonType = cred.IdLogonType;

                db.Entry(dbLocCred).State = EntityState.Modified;

                foreach (var loc in dbLocCred.Locations)
                {
                    foreach (var sourceLoc in loc.TaskLocations)
                    {
                        sourceLoc.Task.LastChanged = DateTime.Now;
                        db.Entry(sourceLoc.Task).State = EntityState.Modified;
                    }

                    foreach (var destLoc in loc.TaskLocations)
                    {
                        destLoc.Task.LastChanged = DateTime.Now;
                        db.Entry(destLoc.Task).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }

            OperationResultMessage = "Credential was successfully updated";
            return RedirectToAction("Index", "AdminLocationCredentials");
        }

        [HttpGet]
        public ActionResult DeleteLocationCredential(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var cred = db.LocationCredentials.Where(x => x.Id == id).Include(x => x.LogonType).FirstOrDefault();
                return View(cred);
            }
        }

        [HttpPost]
        public ActionResult DeleteLocationCredential(Models.LocationCredential cred)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbLocCred = db.LocationCredentials.FirstOrDefault(x => x.Id == cred.Id);
                db.LocationCredentials.Remove(dbLocCred);

                foreach (var loc in dbLocCred.Locations)
                {
                    foreach (var sourceLoc in loc.TaskLocations)
                    {
                        sourceLoc.Task.LastChanged = DateTime.Now;
                        db.Entry(sourceLoc.Task).State = EntityState.Modified;
                    }

                    foreach (var destLoc in loc.TaskLocations)
                    {
                        destLoc.Task.LastChanged = DateTime.Now;
                        db.Entry(destLoc.Task).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();

                OperationResultMessage = "Credential was successfully deleted";
                return RedirectToAction("Index", "AdminLocationCredentials");
            }
        }
    }
}
