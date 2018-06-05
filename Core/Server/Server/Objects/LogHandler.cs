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
    /// <summary>
    /// Slouzi pro praci s logy ze strany API
    /// </summary>
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
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new UniversalLogResponse() { ErrorMessages = Util.EList(new Shared.NetMessages.ErrorMessage() { id = 400, message = "Logs jsou null" }) });

            {
                Authenticator authenticator = new Authenticator(sql);
                if (!authenticator.IsSessionValid(msg.sessionUuid, true))
                    return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Forbidden, new UniversalLogResponse() { ErrorMessages = Util.EList(Authenticator.BAD_SESSION) });
            }

            try
            {
                foreach (var log in msg.Logs)
                {
                    PutLog(log);
                }
                sql.SaveChanges();
            }
            catch (Exception e)
            {//TODO: 
                Console.WriteLine(e.StackTrace);
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.InternalServerError, new UniversalLogResponse() { });
                
            }
            return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Created, new UniversalLogResponse() { });

        }

        private void PutUniversal(JsonableUniversalLog log)
        {
            UniversalLog slog = new UniversalLog()
            {
                Code = log.Code,
                Content = log.Content,
                DateCreated = log.DateCreated,
                IdLogType = (int)log.LogType
            };
            sql.UniversalLogs.Add(slog);
        }

        public void PutLog(JsonableUniversalLog log)
        {
            PutUniversal(log);
        }
    }
}