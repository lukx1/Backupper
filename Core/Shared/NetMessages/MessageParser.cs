using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Nepouživat
    /// </summary>
    [System.Obsolete]
    public static class MessageParser
    {
        

        [System.Obsolete]
        public static IntroductionMessage ParseIntroduction(string json)
        {
            string msg = json.Substring(json.IndexOf('{'));
            return JsonConvert.DeserializeAnonymousType(msg, new IntroductionMessage());
        }

        [System.Obsolete]
        public static ErrorMessage ParseError(string json)
        {
            string msg = json.Substring(json.IndexOf('{'));
            return JsonConvert.DeserializeAnonymousType(msg, new ErrorMessage());
        }

        [System.Obsolete]
        public static PingMessage ParsePing(string json)
        {
            string msg = json.Substring(json.IndexOf('{'));
            return JsonConvert.DeserializeAnonymousType(msg, new PingMessage());
        }

        /*private class MsgTmp
        {
            public string msg;
            public INetMessage tmp;
        }*/

        private static INetMessage FindNetMessageWithUID(uint uid)
        {
            switch (uid)
            {
                case 0:
                    return new PingMessage();
                case 1:
                    return new ErrorMessage();
                case 2:
                    return new IntroductionMessage();
                default:
                    throw new ArgumentException("uid");
            }
        }

        public static bool TryParseMessageType(string json, out Type type)
        {
            uint uid;

            int index = json.IndexOf('{');
            if (!uint.TryParse(json.Substring(0, index), out uid))
            {
                type = null;
                return false;
            }

            type = FindNetMessageWithUID(uid).GetType();
            return true;
        }

        public static Type ParseMessageType(string json)
        {
            uint uid;
            int index = json.IndexOf('{');
            if (!uint.TryParse(json.Substring(0, index), out uid))
                throw new ArgumentException("UID could not be parsed");
            return FindNetMessageWithUID(uid).GetType();
        }

        public static T ParseMessage<T>(string json)
        {
            uint uid;
            int index = json.IndexOf('{');
            if (!uint.TryParse(json.Substring(0, index), out uid))
                throw new ArgumentException("UID could not be parsed");
            string msg = json.Substring(index);
            return JsonConvert.DeserializeAnonymousType(msg, (T)FindNetMessageWithUID(uid));
        }

        /*public static INetMessage ParseMessage(string json)
        {
            uint uid;
            int index = json.IndexOf('{');
            if (!uint.TryParse(json.Substring(0, index), out uid))
                throw new ArgumentException("UID could not be parsed");
            string msg = json.Substring(index);
            return JsonConvert.DeserializeAnonymousType(msg, FindNetMessageWithUID(uid));
        }*/

        /*private static MsgTmp ParseMessage(string json)
        {
            uint uid;
            int index = json.IndexOf('{');
            if (!uint.TryParse(json.Substring(0, index), out uid))
                throw new ArgumentException("UID could not be parsed");
            string msg = json.Substring(index);
            return null;
            //return JsonConvert.DeserializeAnonymousType(msg,(PingMessage)template);
        }*/
    }
}
