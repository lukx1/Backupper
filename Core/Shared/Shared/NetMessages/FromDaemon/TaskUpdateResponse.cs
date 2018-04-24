using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ToDaemon;

namespace Shared.NetMessages.FromDaemon
{
    public class TaskUpdateResponse : IMarkedNetMessage, INetError
    {
        public List<ErrorMessage> ErrorMessages { get; set; }
        public TDMessageCode Code => TDMessageCode.TaskUpdate;
    }
}
