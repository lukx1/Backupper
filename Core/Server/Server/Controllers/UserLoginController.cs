using Server.Models;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class UserLoginController : ApiController
    {
        /// <summary>
        /// Zjistí jestli je login platný
        /// </summary>
        /// <param name="taskMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]UserLoginMessage taskMessage)
        {
            using(MySQLContext sql = new MySQLContext())
            {
                var res = (from users in sql.Users
                                  where users.Nickname == taskMessage.Username
                                  select new { users.Password, users.PrivateKey}).FirstOrDefault();

                if (res != null && PasswordFactory.ComparePasswordsPbkdf2(taskMessage.Password ?? "", res.Password ?? ""))
                    return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new UserLoginResponse() { OK = true,PrivateKeyEncrypted = res.PrivateKey });
                else
                    return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.Forbidden, new UserLoginResponse() { OK = false });
            }
        }
    }
}