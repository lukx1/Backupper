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
	[AdminExc]
	public class AdminTasksController : AdminBaseController
	{
		[HttpGet]
		[AdminSec]
		public ActionResult Tasks(int id)
		{
			using (var db = new Models.MySQLContext())
			{
				var daemon = db.Daemons
					.Where(x => x.Id == id)
					.Include(x => x.User)
					.Include(x => x.Tasks.Select(z => z.TaskDetail))
					.Include(x => x.Tasks.Select(z => z.BackupType))
					.FirstOrDefault();

				return View(daemon);
			}
		}

		[HttpGet]
		[AdminSec]
		public ActionResult TaskTimes(int id)
		{
			var model = new Models.Admin.TaskTimesModel(id);
			model.Load();
			return View(model);
		}

		[HttpPost]
		[AdminSec]
		public ActionResult TaskTimes(Models.Admin.TaskTimesModel model)
		{
			model.Save();
			OperationResultMessage = "Time edition was successfull";
			return RedirectToAction("Tasks", "AdminTasks", new { id = model.IdDaemon });
		}

		[HttpGet]
		[AdminSec]
		public ActionResult NewTask(int id)
		{
			using (var db = new MySQLContext())
			{

				ViewBag.BackupTypes = db.BackupTypes.Select(x => new SelectListItem()
				{
					Value = x.Id.ToString(),
					Text = x.ShortName
				}
				).ToArray();
			}

			return View(new Models.Task() { IdDaemon = id });
		}

		[HttpPost]
		[AdminSec]
		public ActionResult NewTask(Models.Task task)
		{
			using (var db = new Models.MySQLContext())
			{
				task.LastChanged = DateTime.Now;
				db.Tasks.Add(task);
				db.SaveChanges();
			}

			OperationResultMessage = "New task was successfully added";
			return RedirectToAction("Tasks", "AdminTasks", new { id = task.IdDaemon });
		}

		[HttpGet]
		[AdminSec]
		public ActionResult EditTask(int id)
		{
			using (var db = new Models.MySQLContext())
			{
				var task = db.Tasks
					.Include(x => x.TaskDetail)
					.FirstOrDefault(x => x.Id == id);

				ViewBag.BackupTypes = db.BackupTypes.Select(x => new SelectListItem()
				{
					Value = x.Id.ToString(),
					Text = x.ShortName
				}
				).ToArray();

				return View(task);
			}
		}

		[HttpPost]
		[AdminSec]
		public ActionResult EditTask(Models.Task task)
		{
			using (var db = new Models.MySQLContext())
			{
				var dbTask = db.Tasks.FirstOrDefault(x => x.Id == task.Id);

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

			OperationResultMessage = "Task was successfully updated";
			return RedirectToAction("Tasks", "AdminTasks", new { id = task.IdDaemon });
		}

		[HttpGet]
		[AdminSec]
		public ActionResult DeleteTask(int id)
		{
			using (var db = new Models.MySQLContext())
			{
				var task = db.Tasks.Where(x => x.Id == id).Include(x => x.BackupType).FirstOrDefault();
				if (task == null)
				{
					OperationResultMessage = "Task does not exists";
					return RedirectToAction("Index", "AdminDaemons");
				}
				return View(task);
			}
		}

		[HttpPost]
		[AdminSec]
		public ActionResult DeleteTask(Models.Task task)
		{
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

				OperationResultMessage = "Task was successfully deleted";
				return RedirectToAction("Index", "AdminDaemons");
			}
		}

		[HttpGet]
		[AdminSec]
		public ActionResult TaskLocations(int id)
		{
			using (var db = new MySQLContext())
			{
				var model = db.Tasks
					.Where(x => x.Id == id)
					.Include(x => x.TaskLocations.Select(z => z.Location))
					.Include(x => x.TaskLocations.Select(z => z.Location1))
					.FirstOrDefault();

				return View(model);
			}
		}

		[HttpGet]
		[AdminSec]
		public ActionResult NewTaskLocation(int id)
		{
			var model = new Models.Admin.NewEditTaskLocationsModel();
			model.IdTask = id;

			model.Load();

			return View(model);
		}

		[HttpPost]
		[AdminSec]
		public ActionResult NewTaskLocation(Models.Admin.NewEditTaskLocationsModel model)
		{
			model.Save();

			OperationResultMessage = "New task location was successfully added";
			return RedirectToAction("TaskLocations", "AdminTasks", new { id = model.IdTask });
		}

		[HttpGet]
		[AdminSec]
		public ActionResult EditTaskLocation(int id)
		{
			var model = new Models.Admin.NewEditTaskLocationsModel();
			model.Id = id;
			model.Load();

			return View(model);
		}

		[HttpPost]
		[AdminSec]
		public ActionResult EditTaskLocation(Models.Admin.NewEditTaskLocationsModel model)
		{
			if (!Util.IsUserAlreadyLoggedIn(Session))
				return RedirectToAction("Login", "AdminLogin");

			model.Save();

			OperationResultMessage = "Task location was successfully updated";
			return RedirectToAction("TaskLocations", "AdminTasks", new { id = model.IdTask });
		}

		[HttpGet]
		[AdminSec]
		public ActionResult DeleteTaskLocation(int id)
		{
			using (var db = new Models.MySQLContext())
			{
				var loc = db.TaskLocations.Where(x => x.Id == id).Include(x => x.Location).Include(x => x.Location1).FirstOrDefault();
				return View(loc);
			}
		}

		[HttpPost]
		[AdminSec]
		public ActionResult DeleteTaskLocation(Models.TaskLocation loc)
		{
			using (var db = new Models.MySQLContext())
			{
				var dbTask = db.TaskLocations.FirstOrDefault(x => x.Id == loc.Id);

				db.TaskLocations.Remove(dbTask);
				db.SaveChanges();

				OperationResultMessage = "Task location was successfully deleted";
				return RedirectToAction("Index", "AdminDaemons");
			}
		}
	}
}