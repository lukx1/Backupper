using Server.Authentication;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private IntroductionResponse HandleIntroduction(IntroductionMessage message)
        {
            if (authenticator == null)
                authenticator = new DaemonIntroducer();

            authenticator.ReadIntroduction(message);
            if (!authenticator.IsValid())
                return new IntroductionResponse() { errorMessage = new ErrorMessage {message="Údaje nejsou platné" } };
            return authenticator.AddToDBMakeResponse();
        }

        /// <summary>
        /// Přijme introduction od daemonu a ověří
        /// </summary>
        /// <param name="value"></param>
        /// <returns>success pokud prošlo, jinak failure</returns>
        public IntroductionResponse Post([FromBody]IntroductionMessage value)
        {
            return HandleIntroduction(value);
        }

    }
}