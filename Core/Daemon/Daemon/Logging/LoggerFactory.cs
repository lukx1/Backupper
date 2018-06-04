using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{
    /// <summary>
    /// Továrna na loggery
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Podle kontextu vytvoří vhodný logger
        /// </summary>
        /// <returns></returns>
        public static ILogger CreateAppropriate()
        {
            return UniLogger.CreateInstance();
        }
    }
}
