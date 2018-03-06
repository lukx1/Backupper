using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Utility
{
    public interface IConfig
    {
        Guid Uuid { get; set; }
        string Pass { get; set; }
        Guid Session { get; set; }
        DateTime LastCommunicator { get; set; }
    }
}
