using Newtonsoft.Json;
using Shared.ToDaemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public interface IMarkedNetMessage : INetMessage
    {
        [JsonIgnore]
        TDMessageCode Code { get; }
    }
}
