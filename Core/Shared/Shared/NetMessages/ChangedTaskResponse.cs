using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class ChangedTaskResponse : INetMessage, INetError
    {
        public IEnumerable<DbTask> ChangedTasks { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}
