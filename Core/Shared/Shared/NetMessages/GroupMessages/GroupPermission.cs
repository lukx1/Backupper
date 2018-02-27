using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.GroupMessages
{
    public class GroupPermission
    {
        public int Id;
        public Permission Permission;
        public bool? Allow;
        public bool? Deny;
    }
}
