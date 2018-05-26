using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Authentication;

using Newtonsoft.Json;

namespace Server.Controllers
{
	
    [AdminSec(Permission.MANAGEEMAIL)]
	public class AdminEmailController : AdminBaseController
	{
		public ViewResult Index()
		{
			return View();
		}

		[HttpPost]
		public string SendMail(Objects.MailHandler.SendMailProperties model)
		{
			try
			{
				Objects.MailHandler.SendMail(model);
				return "Sent";
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}
	}
}