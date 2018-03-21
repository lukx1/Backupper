using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Logging
{
    public abstract class AbstractLog
    {
        public LogType Type { get; }
        public string Class => this.GetType().Name;
        public abstract string ToJson();
    }
}
