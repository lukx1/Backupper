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
    public class IntroductionController : ApiController
    {
        private DaemonAuthenticator authenticator;

        private INetMessage HandleIntroduction(IntroductionMessage message)
        {
            if (authenticator == null)
                authenticator = new DaemonAuthenticator(new Models.DaemonLogin());
            authenticator.ReadIntroduction(message);
            if (!authenticator.IsValid())
                return new ErrorMessage() { message="Invalid preshared key" };
            return authenticator.AddToDBMakeResponse();
            }

        private INetMessage HandleMessage(Type type, string json)
        {
            if(type == typeof(IntroductionMessage))
            {
                return HandleIntroduction(MessageParser.ParseMessage<IntroductionMessage>(json));
            }
            else
            {
                return new ErrorMessage() { message = "Invalid message type" };
            }
        }

        /**Post*/
        public string Post([FromBody]IntroductionMessage value)
        {
            return HandleIntroduction(value).ToString();
        }

    }
}