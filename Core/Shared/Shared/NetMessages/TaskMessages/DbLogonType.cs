using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class DbLogonType
    {
        public static readonly DbLogonType Anonymous = new DbLogonType(1, "Anonymous");
        public static readonly DbLogonType Normal = new DbLogonType(2, "Normal");
        public static readonly DbLogonType AskForPassword = new DbLogonType(3, "Ask for password");
        public static readonly DbLogonType Interactive = new DbLogonType(4, "Interactive");
        public static readonly DbLogonType Account = new DbLogonType(5, "Account");

        [DeserializeOnly]
        public DbLogonType() { }

        private DbLogonType(int id, string name)
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
