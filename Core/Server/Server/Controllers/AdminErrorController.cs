using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class AdminErrorController : Controller
    {
        public string Index()
        {
            return "Database has been slain.";
        }
    }
}