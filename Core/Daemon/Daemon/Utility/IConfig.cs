using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Utility
{
    public interface IConfig
    {
        string Server { get; set; }
        bool Debug { get; set; }
        Guid Uuid { get; set; }
        string Pass { get; set; }
        Guid Session { get; set; }
        DateTime LastCommunicator { get; set; }
        int SessionLength { get; set; }
        int SessionLengthPadding { get; set; }
    }
}
