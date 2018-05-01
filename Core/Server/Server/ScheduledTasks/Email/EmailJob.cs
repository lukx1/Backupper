using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            });
        }
    }
}