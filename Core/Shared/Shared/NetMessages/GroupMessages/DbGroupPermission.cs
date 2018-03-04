using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.GroupMessages
{
    public class DbGroupPermission
    {
        public int Id;
        public DbPermission Permission;
        public bool? Allow;
        public bool? Deny;
    }
}
