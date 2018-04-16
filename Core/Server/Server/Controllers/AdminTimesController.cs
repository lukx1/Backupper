using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;
using Server.Models;
using Shared;

namespace Server.Controllers
{
	[AdminExc]
	public class AdminTimesController : AdminBaseController
	{
		[AdminSec]
		public ActionResult Index()
		{
			using (var db = new Models.MySQLContext())
				return View(db.Times.ToList());
		}

		[HttpGet]
		[AdminSec]
		public ActionResult NewTime()
		{
			return View(new Models.Time());
		}

		[HttpPost]
		[AdminSec]
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
		[AdminSec]
		public ActionResult EditTime(int id)
		{
			using (var db = new Models.MySQLContext())
			{
				var time = db.Times.FirstOrDefault(x => x.Id == id);
				if (time == null)
				{
					OperationResultMessage = "Time does not exists";
					return RedirectToAction("Index", "AdminTimes");
				}
				return View(time);
			}
		}

		[HttpPost]
		[AdminSec]
		public ActionResult EditTime(Models.Time time)
		{
			using (var db = new Models.MySQLContext())
			{
				db.Times.AddOrUpdate(time);
				db.SaveChanges();
			}

			OperationResultMessage = "Time was successfully updated";
			return RedirectToAction("Index", "AdminTimes");
		}

		[HttpGet]
		[AdminSec]
		public ActionResult DeleteTime(int id)
		{
			using (var db = new Models.MySQLContext())
			{
				var time = db.Times.FirstOrDefault(x => x.Id == id);
				if (time == null)
				{
					OperationResultMessage = "Time does not exists";
					return RedirectToAction("Index", "AdminTimes");
				}
				return View(time);
			}
		}

		[HttpPost]
		[AdminSec]
		public ActionResult DeleteTime(Models.Time time)
		{
			using (var db = new Models.MySQLContext())
			{
				var dbTime = db.Times.FirstOrDefault(x => x.Id == time.Id);
				db.Times.Remove(dbTime);
				db.SaveChanges();

				OperationResultMessage = "Time was successfully deleted";
				return RedirectToAction("Index", "AdminUsers");
			}
		}
	}
}