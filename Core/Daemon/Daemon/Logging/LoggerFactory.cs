using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{
    public static class LoggerFactory
    {

        public static ILogger CreateAppropriate()
        {
            return ConsoleLogger.CreateInstance();
        }
    }
}
