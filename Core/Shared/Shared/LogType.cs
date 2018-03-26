using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public enum LogType
    {
        /// <summary>
        /// System unstable
        /// </summary>
        EMERGENCY = 1,
        /// <summary>
        /// Immediate action needed
        /// </summary>
        ALERT = 2,
        /// <summary>
        /// Critical conditions
        /// </summary>
        CRITICAL = 3,
        /// <summary>
        /// Error conditions
        /// </summary>
        ERROR = 4,
        /// <summary>
        /// Warning conditions
        /// </summary>
        WARNING = 5,
        /// <summary>
        /// Normal but significant condition
        /// </summary>
        NOTIFICATION = 6,
        /// <summary>
        /// Informational messages only
        /// </summary>
        INFORMATION = 7,
        /// <summary>
        /// Debugging messages
        /// </summary>
        DEBUG = 8
    }
}
