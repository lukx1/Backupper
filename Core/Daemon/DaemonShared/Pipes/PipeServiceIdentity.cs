using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonShared.Pipes
{
    public class PipeServiceIdentity
    {
        public string Identity { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static PipeServiceIdentity Deserialize(string s)
        {
            return JsonConvert.DeserializeObject<PipeServiceIdentity>(s);
        }

    }
}
