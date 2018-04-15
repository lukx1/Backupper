using Server.Authentication;
using Server.Models;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class RSAForDaemonController : ApiController
    {

        private HttpResponseMessage PostResponse(RSAForDaemonMessage message)
        {
            if (message.sessionUuid == null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new RSAForDaemonResponse());
            Authenticator auth = new Authenticator();
            if(!auth.IsDaemonSessionValid(message.sessionUuid))
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Unauthorized, new RSAForDaemonResponse());
            var daemon = auth.GetDaemonFromUuid(message.sessionUuid);
            if (!auth.GetPermissionsDaemon(daemon.Uuid).Contains(Authentication.Permission.DAEMONFETCHTASKS))
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Unauthorized, new RSAForDaemonResponse());
            using(MySQLContext sql = new MySQLContext())
            {
                var privK = (from users in sql.Users
                            join daemons in sql.Daemons on users.Id equals daemons.IdUser
                            where daemons.Id == daemon.Id
                            select  users.PrivateKey ).FirstOrDefault();
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new RSAForDaemonResponse() { EncryptedPrivateKey = privK });
            }
        }

        /// <summary>
        /// Získá tasky pro daemona
        /// </summary>
        /// <param name="taskMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]RSAForDaemonMessage message)
        {
            return PostResponse(message);
        }

    }
}