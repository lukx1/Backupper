using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using Quartz.Impl;
using Server.ScheduledTasks.Email;

namespace Server.ScheduledTasks
{
    public class JobScheduler
    {
        public static async Task Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                (s =>
                    s.WithIntervalInHours(24)
                        .OnEveryDay()
                        .StartingDailyAt(new TimeOfDay(0, 0))
                )
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}