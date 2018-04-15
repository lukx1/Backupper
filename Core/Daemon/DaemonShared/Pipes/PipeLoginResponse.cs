using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonShared.Pipes
{
    public class PipeLoginResponse
    {
        public bool B { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static PipeLoginResponse Deserialize(string s)
        {
            return JsonConvert.DeserializeObject<PipeLoginResponse>(s);
        }

    }
}
