using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{
    public interface ILogger
    {
        void Log(string message, LogType logType);
    }
}
