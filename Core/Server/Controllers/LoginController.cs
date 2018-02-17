using Server.Authentication;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class LoginController : ApiController
    {
        private DaemonLoginer daemonLoginer;

        private StandardResponseMessage TryRefreshLogin(LoginMessage loginMessage)
        {
            if (daemonLoginer == null)
                daemonLoginer = new DaemonLoginer();
            Dictionary<string, object> details = new Dictionary<string, object>();
            
            try
            {

                daemonLoginer.RefreshLoginTimer(loginMessage.uuid);
                details.Add("uuid", true);
                details.Add("time", true);
                return new StandardResponseMessage() { type = ResponseType.SUCCESS, message = "Refreshed", values = details };
            }
            catch(InvalidOperationException e)
            {
                details.Add("uuid", true);
                details.Add("time", false);
                return new StandardResponseMessage() { type = ResponseType.FAILURE, message = e.Message, values = details };
            }
            catch(ArgumentException e)
            {
                details.Add("uuid", null);
                details.Add("time", null);
                return new StandardResponseMessage() { type = ResponseType.FAILURE, message = "Refreshed", values = details };
            }
        }

        private StandardResponseMessage TryLogin(LoginMessage loginMessage)
        {
            if (daemonLoginer == null)
                daemonLoginer = new DaemonLoginer();
            Dictionary<string, object> details = new Dictionary<string, object>();
            try
            {
                daemonLoginer.Login(loginMessage.uuid, loginMessage.password);
                details.Add("daemon", true);
                details.Add("password", true);
                return new StandardResponseMessage() { type = ResponseType.SUCCESS, message ="Daemon byl přihlášen", values = details };
            }
            catch(ArgumentNullException e)
            {
                details.Add("daemon", null);
                details.Add("password", null);
                return new StandardResponseMessage() { type = ResponseType.FAILURE, message = e.Message, values = details };
            }
            catch(ArgumentException e)
            {
                details.Add("daemon", true);
                details.Add("password", false);
                return new StandardResponseMessage() { type = ResponseType.FAILURE, message = e.Message, values = details };
            }
        }

        /// <summary>
        /// Znovu načte čas vypršení daemona
        /// </summary>
        /// <param name="loginMessage"></param>
        /// <returns>OK pokud cas resetovan jinak 400+ kod</returns>
        public HttpResponseMessage Patch([FromBody]LoginMessage loginMessage)
        {
            StandardResponseMessage responseMessage = TryRefreshLogin(loginMessage);
            if (responseMessage.type == ResponseType.FAILURE && responseMessage.values["daemon"] == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, responseMessage);
            else if(responseMessage.type == ResponseType.FAILURE)
                return Request.CreateResponse(HttpStatusCode.RequestTimeout, responseMessage);
            else
                return Request.CreateResponse(responseMessage);
        }

        /// <summary>
        /// Přihlásí daemona
        /// </summary>
        /// <param name="loginMessage"></param>
        /// <returns>OK pokud prihlasen jinak 400+ kod</returns>
        public HttpResponseMessage Post([FromBody]LoginMessage loginMessage)
        {
            StandardResponseMessage responseMessage = TryLogin(loginMessage);
            if (responseMessage.type == ResponseType.FAILURE)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, responseMessage);
            else
                return Request.CreateResponse(responseMessage);
        }
    }
}