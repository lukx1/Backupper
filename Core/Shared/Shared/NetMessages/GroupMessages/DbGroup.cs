using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.GroupMessages
{
    /// <summary>
    /// Pro vytváření, upravování a odstraňování skupin
    /// </summary>
    public class DbGroup
    {
        public int Id;
        public string Name;
        public string Description;
        /// <summary>
        /// Pokud ID jsou Daemonů nebo Userů
        /// </summary>
        public bool ForDaemons;
        public List<GroupPermission> permissions;
        public List<int> IdUsersOrDaemons;
    }
}
