using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Tvoření, upravování a odstraňování tasků
    /// </summary>
    public class TaskMessage : SessionMessage
    {
        /// <summary>
        /// List tasků
        /// </summary>
        public bool IsDaemon = true;
        public List<NetMessages.TaskMessages.DbTask> tasks = new List<TaskMessages.DbTask>();
    }
}
