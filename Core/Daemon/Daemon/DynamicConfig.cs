using Daemon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class DynamicConfig : IConfig
    {
        private LoginSettings loginSettings = new LoginSettings();

        public string Server { get => loginSettings.Server; set => loginSettings.Server = value; }
        public Guid Uuid { get => loginSettings.Uuid; set => loginSettings.Uuid = value; }
        public string Pass { get => loginSettings.Password; set => loginSettings.Password =value; }
        public Guid Session { get => loginSettings.SessionUuid; set => loginSettings.SessionUuid = value; }
        public DateTime LastCommunicator { get => loginSettings.LastCommunication; set => loginSettings.LastCommunication=value; }
        public bool Debug { get => loginSettings.Debug; set => loginSettings.Debug =value; }
        public int SessionLength { get => loginSettings.SessionLengthMinutes; set => loginSettings.SessionLengthMinutes = value; }
        public int SessionLengthPadding { get => loginSettings.SessionLengthPaddingMinutes; set => loginSettings.SessionLengthPaddingMinutes = value; }
    }
}
