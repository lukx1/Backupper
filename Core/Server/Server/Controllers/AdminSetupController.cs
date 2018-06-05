using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Configuration;

namespace Server.Controllers
{
    /// <summary>
    /// Použité pro první nastavení serveru. resp. prvního admin účtu a connection stringu
    /// Connection string jde později změnit ServerSettings
    /// </summary>
    public class AdminSetupController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(Objects.ConnectionStringHelper.ConnectionString))
            {
                return RedirectToAction("Index", "Admin");
            }

            return View(new Models.Admin.ServerSetupModel());
        }

        [HttpPost]
        public ActionResult Index(Models.Admin.ServerSetupModel model)
        {
            if (!string.IsNullOrWhiteSpace(Objects.ConnectionStringHelper.ConnectionString))
            {
                return RedirectToAction("Index", "Admin");
            }
            try
            {
                model.Save();
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return RedirectToAction("Index", "AdminSetup");
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}