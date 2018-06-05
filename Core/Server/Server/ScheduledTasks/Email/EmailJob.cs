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
    /// <summary>
    /// Popis emailoveho background tasku a jeho logika
    /// </summary>
    public class EmailJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                try
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
                            message.From = new MailAddress("test@test.test", "TEST FROM ADDRESS");
                            message.To.Add(new MailAddress("test@test.test", "TEST TO ADDRESS"));
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
                                builder.AppendLine("---BACKUPPER REPORT BEGIN---");
                                builder.AppendLine($"{logs.Count} issues in last 24hours");
                                builder.AppendLine("<table>");
                                builder.AppendLine("<tr><th>Log type</th><th>Date</th><th>Content</th></tr>");
                                foreach (var i in logs)
                                {
                                    builder.AppendLine("<tr>");
                                    builder.AppendLine(i.IdLogType.ToString());
                                    builder.AppendLine("</tr>");
                                    builder.AppendLine("<tr>");
                                    builder.AppendLine(i.DateCreated.ToLongDateString());
                                    builder.AppendLine("</tr>");
                                    builder.AppendLine("<tr>");
                                    builder.AppendLine(i.Content);
                                    builder.AppendLine("</tr>");
                                }
                                builder.AppendLine("</table>");
                                builder.AppendLine("---BACKUPPER REPORT END---");

                                message.Body = builder.ToString();
                            }
                            try
                            {
                                client.Send(message);
                            }
                            catch (Exception ex)
                            {
                                Objects.ServerLogger.Alert("Error has occured while sending an email", ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Objects.ServerLogger.Error("Error has occured while sending an email");
                }
            });
        }
    }
}