using DaemonShared.Pipes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ToDaemon
{
    public class TDMessage
    {
        
        public class Message
        {
            public TDMessageCode Code { get; set; }
            public string Content { get; set; }
            public object SerializeToContent { set => Content = JsonConvert.SerializeObject(value); }
        }

        public IEnumerable<byte> Contents { get; private set; }
        public byte[] ContentsAsArray { get => Contents == null ? new byte[0] : Contents.ToArray(); }

        public static string Serialize(object o) => JsonConvert.SerializeObject(o);

        private TDMessage(IEnumerable<byte> c)
        {
            this.Contents = c;
        }

        private TDMessage()
        {
        }

        public static T Deserialize<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

        public static TDMessage FromObject(TDMessage.Message o)
        {
            var s = JsonConvert.SerializeObject(o.Content);
            TDMessage message = new TDMessage();
            var bytes = Encoding.UTF8.GetBytes(s);
            message.Contents = new byte[4] {
                (byte)(bytes.Length >> 24),
                (byte)(bytes.Length >> 16),
                (byte)(bytes.Length >> 8),
                (byte)(bytes.Length >> 0)
            }.Concat(new byte[4] {
                (byte)((int)o.Code >> 24),
                (byte)((int)o.Code >> 16),
                (byte)((int)o.Code >> 8),
                (byte)((int)o.Code >> 0)
                }).Concat(bytes);
            return message;
        }

    }
}
