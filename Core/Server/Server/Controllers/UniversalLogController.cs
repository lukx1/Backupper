using Server.Authentication;
using Server.Models;
using Server.Objects;
using Shared.NetMessages.LogMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class UniversalLogController : ApiController
    {

        private HttpResponseMessage RespPost(UniversalLogMessage msg)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage RespPut(UniversalLogMessage msg)
        {
            using(LogHandler h = new LogHandler(new MySQLContext()))
            {
                return h.Handle(msg);
            }
        }

        /// <summary>
        /// Vytvoří tasky
        /// </summary>
        /// <param name="taskMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Put([FromBody]UniversalLogMessage taskMessage)
        {
            return RespPut(taskMessage);
        }

        /// <summary>
        /// Získá tasky pro daemona
        /// </summary>
        /// <param name="taskMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]UniversalLogMessage taskMessage)
        {
            throw new NotImplementedException();
            //return PostResponse(taskMessage);
        }
    }
}