using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Quartz;

namespace Server.ScheduledTasks.Email
{
    public class EmailJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Objects.ServerLogger.Information("EmailJob at: " + DateTime.Now);

                using (var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("2a13807f026140", "01a60d78457978"),
                    EnableSsl = true
                })
                {
                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress("from@from.from", "TEST FROM ADDRESS");
                        message.To.Add(new MailAddress("to@to.to", "TEST TO ADDRESS"));
                        message.IsBodyHtml = true;

                        message.Subject = "Backupper Report";

                        using (var db = new Models.MySQLContext())
                        {
                            var logs = db.UniversalLogs
                                .Where(x => x.IdLogType <= 5)
                                .ToList()
                                .Where(x => x.DateCreated.AddDays(1) >= DateTime.Now)
                                .ToList();
                            var builder = new StringBuilder();
                            builder.AppendLine("---BACKUPPER REPORT---");
                            builder.AppendLine($"{logs.Count} issues");
                            foreach (var i in logs)
                            {
                                builder.AppendLine(i.Content);
                            }

                            message.Body = builder.ToString();
                        }

                        client.Send(message);
                    }
                }
            });
        }
    }
}