using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class LogonType
    {
        public static readonly LogonType Anonymous = new LogonType(1, "Anonymous");
        public static readonly LogonType Normal = new LogonType(2, "Normal");
        public static readonly LogonType AskForPassword = new LogonType(3, "Ask for password");
        public static readonly LogonType Interactive = new LogonType(4, "Interactive");
        public static readonly LogonType Account = new LogonType(5, "Account");

        [DeserializeOnly]
        public LogonType() { }

        private LogonType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        [DeserializeOnly]
        public int Id;
        [DeserializeOnly]
        public string Name;
    }
}
