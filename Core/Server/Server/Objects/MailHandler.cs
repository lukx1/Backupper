using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Server.Objects
{
    /// <summary>
    /// Pomocna trida pro posilani emailu
    /// </summary>
	public class MailHandler
	{
		public class SendMailProperties
		{
			public string Subject { get; set; }
			public string From { get; set; }
			public string To { get; set; }
			public string Body { get; set; }
			public bool IsBodyHtml { get; set; }

			public string Username { get; set; }
			public string Password { get; set; }
			public string Host { get; set; }
			public int Port { get; set; }
			public bool EnableSsl { get; set; }
		}
		public static void SendMail(SendMailProperties properties)
		{
			var message = new MailMessage();
			message.To.Add(new MailAddress(properties.To));
			message.From = new MailAddress(properties.From);
			message.Subject = properties.Subject;
			message.Body = properties.Body;
			message.IsBodyHtml = properties.IsBodyHtml;

			using (var smtp = new SmtpClient())
			{
				var credential = new NetworkCredential
				{
					UserName = properties.Username,
					Password = properties.Password
				};
				smtp.Credentials = credential;
				smtp.Host = properties.Host;
				smtp.Port = properties.Port;
				smtp.EnableSsl = properties.EnableSsl;
				smtp.SendMailAsync(message);
			}
		}
	}
}