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
        public ActionResult Tasks(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var daemon = db.Daemons
                        .Where(x => x.Id == id)
                        .Include(x => x.User)
                        .Include(x => x.Tasks)
                        .FirstOrDefault();

                    if (daemon == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Daemon does not exists";
                        return RedirectToAction("Index", "AdminDaemons");
                    }
                    return View(daemon);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult NewTask(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }

            return View(new Models.Task() { IdDaemon = id });
        }

        [HttpPost]
        public ActionResult NewTask(Models.Task task)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.Tasks.Add(task);
                    db.SaveChanges();
                }

                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "New task was successfully added";
                return RedirectToAction("Tasks", "AdminTasks", new { id = task.IdDaemon });
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult EditTask(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var task = db.Tasks.FirstOrDefault(x => x.Id == id);
                    if (task == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Task does not exists";
                        return RedirectToAction("Index", "AdminDaemons");
                    }
                    return View(task);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult EditTask(Models.Task task)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.Tasks.AddOrUpdate(task);
                    db.SaveChanges();
                }

                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Task was successfully updated";
                return RedirectToAction("Tasks", "AdminTasks", new { id = task.IdDaemon });
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult DeleteTask(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var task = db.Tasks.FirstOrDefault(x => x.Id == id);
                    if (task == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Task does not exists";
                        return RedirectToAction("Index", "AdminDaemons");
                    }
                    return View(task);
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult DeleteTask(Models.Task task)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var dbTask = db.Tasks.FirstOrDefault(x => x.Id == task.Id);
                    if (dbTask == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Task does not exists";
                        return RedirectToAction("Index", "AdminDaemons");
                    }

                    db.Tasks.Remove(dbTask);
                    db.SaveChanges();

                    TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Task was successfully deleted";
                    return RedirectToAction("Index", "AdminDaemons");
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        //[HttpGet]
        //public ActionResult TaskLocations(int id)
        //{
        //    try
        //    {
        //        if (!Util.IsUserAlreadyLoggedIn(Session))
        //            return RedirectToAction("Login", "AdminLogin");

        //        //var model = new Models.Admin.TaskLocationsModel(id);
        //        //model.Populate();

        //        return View(model);
        //    }
        //    catch (Exception e)
        //    {
        //        //TODO: LOG
        //        TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
        //        return RedirectToAction("Index", "AdminError");
        //    }
        //}
    }
}