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
            { // Opraveno na novejsi verzi DB. Tohle se s heslama nema delat lepsi verze zakomentovana pod(tak ktere necrashne na null)
                using (Models.MySQLContext db = new Models.MySQLContext())
                {
                    var userDetails = db.Users.Where(x => x.Nickname == loginModel.Username && x.Password == loginModel.Password).FirstOrDefault();
                    if (userDetails == null)
                    {
                        return View(new Models.Admin.LoginModel());
                    }
                    else
                    {
                        Session["usedId"] = userDetails.Id;
                        Session["userName"] = userDetails.Nickname;
                        return RedirectToAction("Index", "Admin", null);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); // Null exception?
            }

            return View();
        }

        /*
        //Lepsi verze
         [HttpPost]
        public ActionResult LogIn(Models.Admin.LoginModel loginModel)
        {
            try
            { //Heslo pro Administratora musis prevest z Admin123456 na a8hL7q9GaQQDp60J5Ffvxw==pXCTaEl2U25SBBrhbE83n29WXNXZCh9W56Ug2aS7xSc=
                using (Models.MySQLContext db = new Models.MySQLContext()) // WTF to co to bylo před tim ??? viz PasswordFactory
                {
                    var userDetails = db.Users.Where(x => x.Nickname == loginModel.Username).FirstOrDefault();
                    if(!PasswordFactory.ComparePasswordsPbkdf2(loginModel.Password,userDetails.Password)) 
                        return View(new Models.Admin.LoginModel());// Hesla se neshodují
                    else
                    {
                        Session["usedId"] = userDetails.Id;
                        Session["userName"] = userDetails.Nickname;
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
             */

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("LogIn", "Admin");
        }
    }
}