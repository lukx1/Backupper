using Shared.NetMessages;
using Shared.ToDaemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public abstract class ListenController
    {
        public abstract IMarkedNetMessage Receive(string json);

        protected string Serialize(object o)
        {
            return TDMessage.Serialize(o);
        }

        protected T Deserialize<T>(string s)
        {
            return TDMessage.Deserialize<T>(s);
        }
    }
}
