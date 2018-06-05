using Newtonsoft.Json;
using Server.Models;
using Server.Objects;
using Shared;
using Shared.LogObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Server
{
    /// <summary>
    /// Zajišťuje periodické zapisování uptimu serveru do databáze
    /// </summary>
    public class HostringEnvironmentRegisteredShutdownObject : IRegisteredObject
    {

        private const int period = 5000;
        private int LastStartLogId = -1;

        private Timer Timer; 

        private void MakeOrUpdateNoChangeLog()
        {
            using (MySQLContext sql = new MySQLContext())
            {
                var startLog = (from logs in sql.UniversalLogs
                                where logs.Id == LastStartLogId
                                select logs).FirstOrDefault();
                var updateLog = (from logs in sql.UniversalLogs
                                  where logs.DateCreated > startLog.DateCreated &&  logs.Code == LogContentType.SERVER_STATUS.Uuid
                                  select logs).FirstOrDefault();
                if(updateLog == null)
                {
                    SqlLogger logger = new SqlLogger();
                    var uni = logger.CreateUniversalFromSLog(new ServerStatusLog(ServerStatusLog.ServerStatusInfo.Status.NO_CHANGE));
                    sql.UniversalLogs.Add(uni);
                }
                else
                {
                    updateLog.DateCreated = DateTime.Now;
                    ServerStatusLog.ServerStatusInfo info = JsonableUniversalLog.ParseContent<ServerStatusLog.ServerStatusInfo>(updateLog.Content);
                    if (info.State != ServerStatusLog.ServerStatusInfo.Status.NO_CHANGE)
                        return;
                    updateLog.DateCreated = DateTime.Now;
                    
                }
                sql.SaveChanges();
            }
        }

        private async System.Threading.Tasks.Task Init()
        {
            using(MySQLContext sql = new MySQLContext())
            {
                SqlLogger logger = new SqlLogger();
                var lastLog = (from logs in sql.UniversalLogs
                              orderby logs.Id descending
                              select logs).Take(1).FirstOrDefault();
                if(lastLog != null)
                {
                    var content = JsonableUniversalLog.ParseContent<ServerStatusLog.ServerStatusInfo>(lastLog.Content);
                    if(content.State == ServerStatusLog.ServerStatusInfo.Status.NO_CHANGE)
                    {
                        lastLog.Content = JsonConvert.SerializeObject(new ServerStatusLog.ServerStatusInfo() {State = ServerStatusLog.ServerStatusInfo.Status.SHUTTING_DOWN });
                        sql.SaveChanges();
                    }
                    else if(content.State == ServerStatusLog.ServerStatusInfo.Status.STARTING)
                    {
                        sql.UniversalLogs.Add(logger.CreateUniversalFromSLog(new ServerStatusLog(ServerStatusLog.ServerStatusInfo.Status.SHUTTING_DOWN) { DateCreated = lastLog.DateCreated.AddSeconds(1)}));
                        sql.SaveChanges();
                    }
                }
                
                var uni = logger.CreateUniversalFromSLog(new ServerStatusLog(ServerStatusLog.ServerStatusInfo.Status.STARTING));
                sql.UniversalLogs.Add(uni);
                await sql.SaveChangesAsync();

                LastStartLogId = uni.Id;
                if (LastStartLogId == -1)
                    throw new InvalidOperationException("Nelze začít kontrolovat stav serveru");
                
            }
            Timer = new Timer((e) =>
            {
                try
                {
                    MakeOrUpdateNoChangeLog();
                }
                catch (Exception) { }
            }, null, period, period
                );
        }

        public HostringEnvironmentRegisteredShutdownObject()
        {
            var a = Init();
        }

        private async Task<int> SubmitLogAsync(ServerStatusLog.ServerStatusInfo.Status status)
        {
            using (MySQLContext sql = new MySQLContext())
            {
                SqlLogger logger = new SqlLogger();
                return await logger.SubmitLogAsync(new ServerStatusLog(status));
            }
        }

        public void Stop(bool immediate)
        {
            if (!immediate)
            {
                var task = System.Threading.Tasks.Task.Run(()=>SubmitLogAsync(ServerStatusLog.ServerStatusInfo.Status.SHUTTING_DOWN));
            }

        }
    }
}