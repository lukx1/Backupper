using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminGroupsController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                    return View(db.Groups.ToList());
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult GroupPermissions(int id)
        {
            try
            {
                var model = new Models.Admin.GroupPermissionsModel(id);
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
        public ActionResult GroupPermissions(Models.Admin.GroupPermissionsModel model)
        {
            try
            {
                model.Save();
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Permission edition was successfull";
                return RedirectToAction("Index", "AdminGroups");
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult NewGroup()
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

            return View(new Models.Group());
        }

        [HttpPost]
        public ActionResult NewGroup(Models.Group group)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.Groups.Add(group);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }

            TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "New group was successfully created";
            return RedirectToAction("Index", "AdminGroups");
        }

        [HttpGet]
        public ActionResult EditGroup(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var group = db.Groups.FirstOrDefault(x => x.Id == id);
                    if (group == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Group does not exists";
                        return RedirectToAction("Index", "AdminGroups");
                    }
                    return View(group);
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
        public ActionResult EditGroup(Models.Group group)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    db.Groups.AddOrUpdate(group);
                    db.SaveChanges();
                }

                TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Group was successfully updated";
                return RedirectToAction("Index", "AdminGroups");
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }

        [HttpGet]
        public ActionResult DeleteGroup(int id)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var group = db.Groups.FirstOrDefault(x => x.Id == id);
                    if (group == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Group does not exists";
                        return RedirectToAction("Index", "AdminGroups");
                    }
                    return View(group);
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
        public ActionResult DeleteGroup(Models.Group group)
        {
            try
            {
                if (!Util.IsUserAlreadyLoggedIn(Session))
                    return RedirectToAction("Login", "AdminLogin");

                using (var db = new Models.MySQLContext())
                {
                    var dbUser = db.Groups.FirstOrDefault(x => x.Id == group.Id);
                    if (dbUser == null)
                    {
                        TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Group does not exists";
                        return RedirectToAction("Index", "AdminGroups");
                    }

                    db.Groups.Remove(dbUser);
                    db.SaveChanges();

                    TempData[Objects.MagicStrings.OPERATION_RESULT_MESSAGE] = "Group was successfully deleted";
                    return RedirectToAction("Index", "AdminGroups");
                }
            }
            catch (Exception e)
            {
                //TODO: LOG
                TempData[Objects.MagicStrings.ERROR_MESSAGE] = e.Message;
                return RedirectToAction("Index", "AdminError");
            }
        }
    }
}