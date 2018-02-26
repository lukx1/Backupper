using Server.Authentication;
using Server.Objects;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class TaskController : ApiController
    {

        private Authenticator authenticator;
        private TaskHandler taskHandler;

        private HttpResponseMessage PostResponse(TaskMessage taskMessage)
        {
            if (taskMessage == null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.UnsupportedMediaType, new TaskResponse() { ErrorMessages = new List<ErrorMessage>() { new ErrorMessage() { id = 4, message = "Přijumtá zpráva je prázdná" } } });

            if (authenticator == null)
                authenticator = new Authenticator();

            if (!authenticator.IsSessionValid(taskMessage.sessionUuid))
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Unauthorized, new TaskResponse() { ErrorMessages = new List<ErrorMessage>() { new ErrorMessage() { id = 5, message = "Sezení není platné" } } });

            if (taskHandler == null)
                taskHandler = new TaskHandler();

            var errors = taskHandler.CreateTasks(taskMessage);
            if (errors.Any(r => r != null))
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new TaskResponse() { ErrorMessages = errors.ToList() });
            else
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Created, new TaskResponse() { ErrorMessages = new List<ErrorMessage>() });

        }

        /// <summary>
        /// Create tasks
        /// </summary>
        /// <param name="loginMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]TaskMessage loginMessage)
        {
            return PostResponse(loginMessage);
        }
    }
}