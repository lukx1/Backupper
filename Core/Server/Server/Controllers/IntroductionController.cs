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

        private IntroductionResponse OneClick(IntroductionMessage msg, DaemonIntroducer introducer)
        {

                var wwwaiting = (from infos in mySQL.DaemonInfos
                             join wwaiting in mySQL.WaitingForOneClicks on infos.Id equals wwaiting.IdDaemonInfo
                             where infos.PCUuid == msg.PCUuid
                             select wwaiting).FirstOrDefault();

            if (wwwaiting == null)
            {
                DaemonInfo info = new DaemonInfo() { Mac = new string(msg.macAdress), Os = msg.os, PCUuid = msg.PCUuid, DateAdded = DateTime.Now, PcName = msg.pcName, Name = "" };
                WaitingForOneClick waiting = new WaitingForOneClick() { DaemonInfo = info, User = msg.User,DateReceived = DateTime.Now };
                mySQL.DaemonInfos.Add(info);
                mySQL.WaitingForOneClicks.Add(waiting);
                mySQL.SaveChanges();
                return new IntroductionResponse() { WaitForIntroduction = true, WaitID = waiting.Id };
            }
            return new IntroductionResponse() { WaitForIntroduction = true, WaitID = wwwaiting.Id };
        }

        private IntroductionResponse HandleIntroduction(IntroductionMessage message)
        {
            if (message == null)
                return IntroErrMaker(HttpStatusCode.BadRequest, "message = null");

            if (authenticator == null)
                authenticator = new DaemonIntroducer((mySQL = new MySQLContext()));

            if((message.preSharedKey == null || message.preSharedKey.Trim() == "") && true)
            {
                return OneClick(message,authenticator);
            }
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