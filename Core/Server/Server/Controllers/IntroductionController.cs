using Server.Authentication;
using Server.Models;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private IntroductionResponse IntroErrMaker(HttpStatusCode code, string msg)
        {
            IntroductionResponse response = new IntroductionResponse();
            response.ErrorMessages = new List<ErrorMessage>();
            response.ErrorMessages.Add(new ErrorMessage() { id = (int)code, message = msg });
            return response;
        }

        private IntroductionResponse HandleIntroduction(IntroductionMessage message)
        {
            if (message == null)
                return IntroErrMaker(HttpStatusCode.BadRequest, "message = null");

                

            if (authenticator == null)
                authenticator = new DaemonIntroducer((mySQL = new MySQLContext()));

            authenticator.ReadIntroduction(message);
            if (!authenticator.IsValid())
            {
                return IntroErrMaker(HttpStatusCode.Forbidden,"Neplatné údaje");
            }
            return authenticator.AddToDBMakeResponse();
        }

        /// <summary>
        /// Přijme introduction od daemonu a ověří
        /// </summary>
        /// <param name="value"></param>
        /// <returns>success pokud prošlo, jinak failure</returns>
        public HttpResponseMessage Put([FromBody]IntroductionMessage value)
        {
            var msg = HandleIntroduction(value);
            if (msg.ErrorMessages != null && msg.ErrorMessages.Count > 0)
                return Util.MakeHttpResponseMessage<IntroductionResponse>((HttpStatusCode)msg.ErrorMessages[0].id,msg);
            return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Created, msg);
        }

    }
}