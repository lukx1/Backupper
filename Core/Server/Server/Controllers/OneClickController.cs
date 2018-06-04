using Server.Models;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class OneClickController : ApiController
    {
        /// <summary>
        /// Daemon se dozví jestli byl povolen adminem
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Pokud je povolen je mu přiděleno uuid s heslem, pokud ne tak je mu přiděleno nic</returns>
        public HttpResponseMessage Post([FromBody]OneClickMessage msg)
        {
            using(MySQLContext sql = new MySQLContext())
            {
                var sadsda = (from waiting in sql.WaitingForOneClicks
                             select waiting).ToList();
                var wwaiting = (from waiting in sql.WaitingForOneClicks where waiting.Id == msg.ID && waiting.Confirmed select waiting).FirstOrDefault();
                if (wwaiting == null)
                    return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Forbidden, new OneClickResponse() { });
                var unhashedPass = PasswordFactory.CreateRandomPassword(16);
                var hashedPass = PasswordFactory.HashPasswordPbkdf2(unhashedPass);
                var user = (from users in sql.Users where users.Nickname == wwaiting.User select users).FirstOrDefault();
                Daemon daemon = new Daemon() { Uuid = Guid.NewGuid(), Password = hashedPass,IdDaemonInfo = wwaiting.IdDaemonInfo, User = user, };
                var perms = new Collection<DaemonGroup>();
                perms.Add(new DaemonGroup() { Daemon = daemon,IdGroup = 2});
                daemon.DaemonGroups = perms;
                sql.WaitingForOneClicks.Remove(wwaiting);
                sql.Daemons.Add(daemon);
                sql.SaveChanges();
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new OneClickResponse() { password = unhashedPass, uuid = daemon.Uuid });
            }
        }
    }
}