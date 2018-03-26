using Server.Authentication;
using Server.Models;
using Shared;
using Shared.NetMessages.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Server.Objects
{
    public class LogHandler : IDisposable
    {

        public MySQLContext sql;

        public LogHandler(MySQLContext sql)
        {
            this.sql = sql;
        }

        public void Dispose()
        {
            sql.Dispose();
        }

        public HttpResponseMessage Handle(UniversalLogMessage msg)
        {
            if (msg.Logs == null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new SpecificLogResponse() { ErrorMessages = Util.EList(new Shared.NetMessages.ErrorMessage() { id = 400, message = "Logs jsou null" }) });

            {
                Authenticator authenticator = new Authenticator(sql);
                if (!authenticator.IsSessionValid(msg.sessionUuid, true))
                    return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Forbidden, new SpecificLogResponse() { ErrorMessages = Util.EList(Authenticator.BAD_SESSION) });
            }

            try
            {
                foreach (var log in msg.Logs)
                {
                    PutLog(log);
                }
                SaveAsync().Wait();
            }
            catch (Exception e)
            {//TODO: Log
                Console.WriteLine(e.StackTrace);
            }
            return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Created, new SpecificLogResponse() { });
        }

        public HttpResponseMessage Handle(SpecificLogMessage msg)
        {
            if (msg.Logs == null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new SpecificLogResponse() { ErrorMessages = Util.EList(new Shared.NetMessages.ErrorMessage() { id = 400, message = "Logs jsou null" }) });

            {
                Authenticator authenticator = new Authenticator(sql);
                if (!authenticator.IsSessionValid(msg.sessionUuid, true))
                    return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Forbidden, new SpecificLogResponse() { ErrorMessages = Util.EList(Authenticator.BAD_SESSION) });
            }

            try
            {
                foreach (var log in msg.Logs)
                {
                    PutLog(log);
                }
                SaveAsync();
            }
            catch (Exception e)
            {//TODO: Log
                Console.WriteLine(e.StackTrace);
            }
            return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Created, new SpecificLogResponse() { });
        }

        private void PutDaemon(JsonableSpecificLog log)
        {
            DaemonLog slog = new DaemonLog()
            {
                Code = log.Code,
                Content = log.Content,
                DateCreated = log.DateCreated,
                Header = log.Header,
                IdDaemon = log.BoundId,
                IdLogType = (int)log.LogType
            };
            sql.DaemonLogs.Add(slog);
        }

        private void PutTaskLocation(JsonableSpecificLog log)
        {
            TaskLocationLog slog = new TaskLocationLog()
            {
                Code = log.Code,
                Content = log.Content,
                DateCreated = log.DateCreated,
                Header = log.Header,
                IdTaskLocation = log.BoundId,
                IdLogType = (int)log.LogType
            };
            sql.TaskLocationLogs.Add(slog);
        }

        private void PutUser(JsonableSpecificLog log)
        {
            UserLog slog = new UserLog()
            {
                Code = log.Code,
                Content = log.Content,
                DateCreated = log.DateCreated,
                Header = log.Header,
                IdUser = log.BoundId,
                IdLogType = (int)log.LogType
            };
            sql.UserLogs.Add(slog);
        }

        private void PutUniversal(JsonableUniversalLog log)
        {
            UniversalLog slog = new UniversalLog()
            {
                Code = log.Code,
                Content = log.Content,
                DateCreated = log.DateCreated,
                Header = log.Header,
                IdLogType = (int)log.LogType
            };
            sql.UniversalLogs.Add(slog);
        }

        public void PutLog(JsonableSpecificLog log)
        {
            switch (log.LogTable)
            {
                case LogTable.DAEMON:
                    PutDaemon(log);
                    break;
                case LogTable.TASK_LOCATION:
                    PutTaskLocation(log);
                    break;
                case LogTable.USER:
                    PutUser(log);
                    break;
                case LogTable.UNIVERSAL:
                    throw new ArgumentException("Tento kontroler nepodporuje universal log");
                default:
                    throw new ArgumentException("Neočekávaný parametr LogTable");
            }
        }

        public async Task<int> SaveAsync()
        {
            return await sql.SaveChangesAsync();
        }

        public void PutLog(JsonableUniversalLog log)
        {
            PutUniversal(log);
        }
    }
}