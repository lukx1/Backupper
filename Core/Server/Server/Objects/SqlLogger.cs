using Server.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Objects
{
    public class SqlLogger
    {
        private IEnumerable<UniversalLog> CreateUnisFromLogs<T>(params SLog<T>[] logs) where T : class
        {
            foreach (var log in logs)
            {
                yield return CreateUniversalFromSLog(log);
            }
        }

        public UniversalLog CreateUniversalFromSLog<T>(SLog<T> log) where T : class
        {
            var jLog = JsonableUniversalLog.CreateFrom(log);
            var uLog = new UniversalLog()
            {
                Code = jLog.Code,
                Content = jLog.Content,
                DateCreated = jLog.DateCreated,
                IdLogType = (int)jLog.LogType
            };
            return uLog;
        }

        public int SubmitLog<T>(params SLog<T>[] logs) where T : class
        {

            using (MySQLContext sql = new MySQLContext())
            {
                foreach (var uni in CreateUnisFromLogs(logs))
                {
                    sql.UniversalLogs.Add(uni);
                }
                return sql.SaveChanges();
            }
        }

        public async System.Threading.Tasks.Task<int> SubmitLogAsync<T>(params SLog<T>[] logs) where T : class
        {

            using (MySQLContext sql = new MySQLContext())
            {
                foreach (var uni in CreateUnisFromLogs(logs))
                {
                    sql.UniversalLogs.Add(uni);
                }
                return await sql.SaveChangesAsync();
            }
        }

        public SqlLogger()
        {

        }
    }
}