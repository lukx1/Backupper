using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if(Util.IsUserAlreadyLoggedIn(Session))
                return View();
            return RedirectToAction("LogIn", "Admin", null);
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            if(Util.IsUserAlreadyLoggedIn(Session))
                return RedirectToAction("Index", "Admin", null);
            return View(new Models.Admin.LoginModel());
        }


        [HttpPost]
        public ActionResult LogIn(Models.Admin.LoginModel loginModel)
        {
            try
            {
                using (Models.MySQLContext db = new Models.MySQLContext())
                {
                    var userDetails = db.users.Where(x => x.Name == loginModel.Username && x.password == loginModel.Password).FirstOrDefault();
                    if (userDetails == null)
                    {
                        return View(new Models.Admin.LoginModel());
                    }
                    else
                    {
                        Session["usedId"] = userDetails.id;
                        Session["userName"] = userDetails.Name;
                        return RedirectToAction("Index", "Admin", null);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("LogIn", "Admin");
        }
    }
}