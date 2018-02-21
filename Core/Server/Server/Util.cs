using Newtonsoft.Json;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Server
{
    public static class Util
    {
        public static HttpResponseMessage MakeHttpResponseMessage<T>(HttpStatusCode statusCode,T message) where T:INetMessage
        {
            return new HttpResponseMessage(statusCode) { Content = new StringContent(JsonConvert.SerializeObject(message)) };
        }
    }
}