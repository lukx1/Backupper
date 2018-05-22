using Server.Models;
using Shared;
using Shared.LogObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Server.Objects
{
    public class UptimeCalculator
    {
        public class Uptime
        {
            public TimeSpan TimeOnline { get; set; } = new TimeSpan();
            public TimeSpan TimeOffline { get; set; } = new TimeSpan();
            public double UptimePercent { get; set; } = -1;
            public DateTime MeasurementStart { get; set; }
        }

        private bool IsEnd(ServerStatusLog.ServerStatusInfo.Status status)
        {
            switch (status)
            {

                case ServerStatusLog.ServerStatusInfo.Status.TURNING_OFF:
                case ServerStatusLog.ServerStatusInfo.Status.RESTARTING:
                case ServerStatusLog.ServerStatusInfo.Status.SHUTTING_DOWN:
                case ServerStatusLog.ServerStatusInfo.Status.EXITING:
                    return true;
                case ServerStatusLog.ServerStatusInfo.Status.STARTING:
                case ServerStatusLog.ServerStatusInfo.Status.NO_CHANGE:
                    return false;
                default:
                    return false;
            }
        }

        public Uptime Calculate()
        {
            using (MySQLContext sql = new MySQLContext())
            {
                Uptime uptime = new Uptime();

                var statusLogs = from logs in sql.UniversalLogs
                                 where logs.Code == LogContentType.SERVER_STATUS.Uuid
                                 orderby logs.DateCreated ascending
                                 select logs;

                ulong iterator = 0;
                DateTime start = DateTime.MinValue;
                DateTime lastDate = DateTime.MinValue;
                foreach (var log in statusLogs)
                {
                    lastDate = log.DateCreated;
                    if (iterator == 0)
                    {
                        uptime.MeasurementStart = log.DateCreated;
                        start = uptime.MeasurementStart;
                    }
                    else
                    {
                        var content = JsonableUniversalLog.ParseContent<ServerStatusLog.ServerStatusInfo>(log.Content);
                        if (content.State == ServerStatusLog.ServerStatusInfo.Status.NO_CHANGE) { }
                        else if (IsEnd(content.State))
                        {
                            uptime.TimeOnline += (log.DateCreated - start);
                        }
                        else
                        {
                            start = log.DateCreated;
                        }
                    }


                    iterator++;
                }
                uptime.TimeOffline = (lastDate - uptime.MeasurementStart);
                uptime.UptimePercent = uptime.TimeOnline.TotalMinutes / (uptime.TimeOffline + uptime.TimeOnline).TotalMinutes;
                return uptime;
            }

        }

    }
}