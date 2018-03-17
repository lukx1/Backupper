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

        private LoginResponse TryLogin(LoginMessage loginMessage)
        {
            if (daemonLoginer == null)
                daemonLoginer = new DaemonLoginer();
            return daemonLoginer.LoginAndGetSessionUuid(loginMessage.uuid, loginMessage.password);
        }

        /// <summary>
        /// Přihlásí daemona
        /// </summary>
        /// <param name="loginMessage"></param>
        /// <returns>OK pokud prihlasen jinak 400+ kod</returns>
        public HttpResponseMessage Post([FromBody]LoginMessage loginMessage)
        {
            LoginResponse responseMessage = TryLogin(loginMessage);
            if(responseMessage.ErrorMessages != null)
                return Util.MakeHttpResponseMessage((HttpStatusCode)responseMessage.ErrorMessages[0].id,responseMessage);
            return Util.MakeHttpResponseMessage(HttpStatusCode.Created, responseMessage);
        }
    }
}