using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminTasksController : Controller
    {
        [HttpGet]
        public ActionResult NewTask(int id)
        {
            return View(new Models.Task() { IdDaemon = id });
        }

        [HttpPost]
        public ActionResult NewTask(Models.Task task)
        {
            try
            {
                using (var db = new Models.MySQLContext())
                {
                    db.Tasks.Add(task);
                    db.SaveChanges();
                }

                TempData["customMessage"] = "Adding was success";
                return RedirectToAction("DaemonInfo", "AdminDaemons", new { id = task.IdDaemon });
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult EditTask(int id)
        {
            try
            {
                using (var db = new Models.MySQLContext())
                {
                    var task = db.Tasks.FirstOrDefault(x => x.Id == id);
                    if (task == null)
                        throw new Exception("ERROR: Task does not exists");
                    return View(task);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult EditTask(Models.Task task)
        {
            try
            {
                using (var db = new Models.MySQLContext())
                {
                    db.Tasks.AddOrUpdate(task);
                    db.SaveChanges();
                }

                TempData["customMessage"] = "Update was success";
                return RedirectToAction("DaemonInfo", "AdminDaemons", new { id = task.IdDaemon });
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult DeleteTask(int id)
        {
            try
            {
                using (var db = new Models.MySQLContext())
                {
                    var task = db.Tasks.FirstOrDefault(x => x.Id == id);
                    if (task == null)
                        throw new Exception("ERROR: Task does not exists");
                    return View(task);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult DeleteTask(Models.Task task)
        {
            try
            {
                using (var db = new Models.MySQLContext())
                {
                    var dbTask = db.Tasks.FirstOrDefault(x => x.Id == task.Id);
                    if(dbTask == null)
                        throw new Exception("ERROR: Task does not exists");

                    db.Tasks.Remove(dbTask);
                    db.SaveChanges();

                    TempData["customMessage"] = "Deletion was success";
                    return RedirectToAction("Index", "AdminDaemons");
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult TaskLocations(int id)
        {
            try
            {
                var model = new Models.Admin.TaskLocationsModel(id);
                model.Populate();

                return View(model);
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }

        public ActionResult Index()
        {
            try
            {
                using (var db = new Models.MySQLContext())
                    return View(db.Tasks.ToList());
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
                    return View(db.TaskLocationLogs.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                return RedirectToAction("Index", "AdminError");
            }
        }
    }
}