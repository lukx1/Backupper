using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;
using Server.Models;
using Shared;

namespace Server.Controllers
{
    
    [AdminSec(Authentication.Permission.MANAGETIMES)]
    public class AdminTimesController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new Models.MySQLContext())
                return View(db.Times.ToList());
        }

        [HttpGet]
        public ActionResult NewTime()
        {
            return View(new Models.Time());
        }

        [HttpPost]
        public ActionResult NewTime(Models.Time time)
        {
            using (var db = new Models.MySQLContext())
            {
                db.Times.Add(time);
                db.SaveChanges();
            }

            OperationResultMessage = "New time was successfully created";
            return RedirectToAction("Index", "AdminTimes");
        }

        [HttpGet]
        public ActionResult EditTime(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var time = db.Times.FirstOrDefault(x => x.Id == id);
                return View(time);
            }
        }

        [HttpPost]
        public ActionResult EditTime(Models.Time time)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbTime = db.Times.FirstOrDefault(x => x.Id == time.Id);
                dbTime.StartTime = time.StartTime;
                dbTime.EndTime = time.EndTime;
                dbTime.Interval = time.Interval;
                dbTime.Name = time.Name;
                dbTime.Repeat = time.Repeat;
                db.Entry(dbTime).State = EntityState.Modified;

                foreach (var taskTime in dbTime.TaskTimes)
                {
                    taskTime.Task.LastChanged = DateTime.Now;
                    db.Entry(taskTime.Task).State = EntityState.Modified;
                }

                db.SaveChanges();
            }

            OperationResultMessage = "Time was successfully updated";
            return RedirectToAction("Index", "AdminTimes");
        }

        [HttpGet]
        public ActionResult DeleteTime(int id)
        {
            using (var db = new Models.MySQLContext())
            {
                var time = db.Times.FirstOrDefault(x => x.Id == id);
                return View(time);
            }
        }

        [HttpPost]
        public ActionResult DeleteTime(Models.Time time)
        {
            using (var db = new Models.MySQLContext())
            {
                var dbTime = db.Times.FirstOrDefault(x => x.Id == time.Id);

                foreach (var taskTime in dbTime.TaskTimes)
                {
                    taskTime.Task.LastChanged = DateTime.Now;
                    db.Entry(taskTime.Task).State = EntityState.Modified;
                }

                db.Times.Remove(dbTime);
                db.SaveChanges();
            }

            OperationResultMessage = "Time was successfully deleted";
            return RedirectToAction("Index", "AdminUsers");
        }
    }
}