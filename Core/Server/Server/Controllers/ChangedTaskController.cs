using Server.Authentication;
using Server.Models;
using Server.Objects;
using Shared.NetMessages;
using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class ChangedTaskController : ApiController
    {

        /// <summary>
        /// Získá tasky pro daemona
        /// </summary>
        /// <param name="taskMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]ChangedTaskMessage msg)
        {
            if (msg == null || msg.TaskID == null || msg.TaskID.Count() < 1 || msg.sessionUuid == null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new ChangedTaskResponse() { });
            Authenticator auth = new Authenticator();
            if (!auth.IsDaemonSessionValid(msg.sessionUuid))
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Forbidden, new ChangedTaskResponse() { });
            if(!auth.IsDaemonAllowed(auth.GetDaemonFromUuid(msg.sessionUuid).Uuid, Authentication.Permission.DAEMONFETCHTASKS))
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Unauthorized, new ChangedTaskResponse() { });
            if(!auth.AreRequestedTasksDaemons(msg.TaskID,msg.sessionUuid))
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Unauthorized, new ChangedTaskResponse() { });
            using (MySQLContext sql = new MySQLContext()) {
                var recTasks = (from tasks in sql.Tasks
                               where tasks.LastChanged > msg.LastLoaded
                               select tasks);
                TaskHandler handler = new TaskHandler();
                List<DbTask> dbTasks = new List<DbTask>();
                foreach (var item in recTasks)
                {
                    dbTasks.Add(handler.ExtractDataFromTask(item));
                }
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new ChangedTaskResponse() {ChangedTasks = dbTasks });
            }
        }

    }
}