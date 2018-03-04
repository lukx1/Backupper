using Server.Authentication;
using Server.Models;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Server.Controllers
{
    /// <summary>
    /// Vytváří daemony z preshared klíče a id, registruje vstupní info
    /// </summary>
    public class IntroductionController : ApiController
    {
        private DaemonIntroducer authenticator;
        private MySQLContext mySQL;

        private IntroductionResponse HandleIntroduction(IntroductionMessage message)
        {
            if (message == null)
                return new IntroductionResponse() { errorMessage = { id = 1, message = "Přijatý message byl null" } };

            if (authenticator == null)
                authenticator = new DaemonIntroducer((mySQL = new MySQLContext()));

            authenticator.ReadIntroduction(message);
            if (!authenticator.IsValid())
                return new IntroductionResponse() { errorMessage = new ErrorMessage {id = 2,message="Údaje nejsou platné" } };
            return authenticator.AddToDBMakeResponse();
        }

        /// <summary>
        /// Přijme introduction od daemonu a ověří
        /// </summary>
        /// <param name="value"></param>
        /// <returns>success pokud prošlo, jinak failure</returns>
        public HttpResponseMessage Post([FromBody]IntroductionMessage value)
        {
            var msg = HandleIntroduction(value);
            if (msg.errorMessage != null && msg.errorMessage.id == 1)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest,msg);
            else if (msg.errorMessage != null && msg.errorMessage.id == 2)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Forbidden, msg);
            return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Created, msg);
        }

    }
}