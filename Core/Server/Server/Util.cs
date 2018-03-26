using Newtonsoft.Json;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Server
{
    public static class Util
    {

        public static HttpResponseMessage MakeHttpResponseMessage<T>(HttpStatusCode statusCode,T message) where T:INetMessage
        {
            return new HttpResponseMessage(statusCode) { Content = new StringContent(JsonConvert.SerializeObject(message)) };
        }

        public static bool IsUserAlreadyLoggedIn(HttpSessionStateBase session, bool refresh = true)
        {
            bool result;

            Guid? uuid = (Guid?)session[Objects.MagicStrings.SESSION_UUID];
            if (!uuid.HasValue)
                return false;

            using (var db = new Models.MySQLContext())
            {
                var auth = new Authentication.Authenticator(db);
                result = auth.IsUserSessionValid(uuid.Value, true);
            }

            return result;
        }

        public static bool IsExpired(DateTime expires) => expires <= DateTime.Now;
    }
}