using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonShared.Pipes
{
    public class PipeLoginAttempt
    {
        public string U { get; set; }
        public string P { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static PipeLoginAttempt Deserialize(string s)
        {
            return JsonConvert.DeserializeObject<PipeLoginAttempt>(s);
        }

    }
}
