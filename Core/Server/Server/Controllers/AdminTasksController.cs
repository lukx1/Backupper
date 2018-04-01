using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Models;

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
                        .Include(x => x.Tasks.Select(z => z.TaskDetail))
                        .Include(x => x.Tasks.Select(z => z.BackupType))
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
        public ActionResult TaskTimes(int id)
        {
            try
            {
                var model = new Models.Admin.TaskTimesModel(id);
                model.Load();
                return View(model);
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpPost]
        public ActionResult TaskTimes(Models.Admin.TaskTimesModel model)
        {
            try
            {
                model.Save();
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Time edition was successfull";
                return RedirectToAction("Tasks", "AdminTasks", new {id = model.IdDaemon});
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        //TODO: CUSTOM MODEL
        [HttpGet]
        public ActionResult NewTask(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new MySQLContext())
                {

                    ViewBag.BackupTypes = db.BackupTypes.Select(x => new SelectListItem()
                        {
                            Value = x.Id.ToString(),
                            Text = x.ShortName
                        }
                    ).ToArray();
                }
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
                    task.LastChanged = DateTime.Now;
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

        //TODO: CUSTOM MODEL
        [HttpGet]
        public ActionResult EditTask(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var task = db.Tasks
                        .Include(x => x.TaskDetail)
                        .FirstOrDefault(x => x.Id == id);
                    if (task == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Task does not exists";
                        return RedirectToAction("Index", "AdminDaemons");
                    }

                    ViewBag.BackupTypes = db.BackupTypes.Select(x => new SelectListItem()
                        {
                            Value = x.Id.ToString(),
                            Text = x.ShortName
                        }
                    ).ToArray();

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
                    var dbTask = db.Tasks.FirstOrDefault(x => x.Id == task.Id);

                    if (dbTask == null)
                        throw new Exception("Task does not exists");

                    dbTask.IdBackupTypes = task.IdBackupTypes;
                    dbTask.Name = task.Name;
                    dbTask.Description = task.Description;
                    dbTask.TaskDetail.ZipAlgorithm = task.TaskDetail.ZipAlgorithm;
                    dbTask.TaskDetail.CompressionLevel = task.TaskDetail.CompressionLevel;
                    dbTask.LastChanged = DateTime.Now;

                    db.Entry(dbTask.TaskDetail).State = EntityState.Modified;
                    db.Entry(dbTask).State = EntityState.Modified;

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
                    var task = db.Tasks.Where(x => x.Id == id).Include(x => x.BackupType).FirstOrDefault();
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