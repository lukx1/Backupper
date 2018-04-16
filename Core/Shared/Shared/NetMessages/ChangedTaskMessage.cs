using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class ChangedTaskMessage : SessionMessage
    {

        public IEnumerable<int> TaskID{ get; set; }
        public DateTime LastLoaded { get; set; }
    }
}
