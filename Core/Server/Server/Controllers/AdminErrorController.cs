﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Controllers
{
    [AdminExc]
    public class AdminErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}