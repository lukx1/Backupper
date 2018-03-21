using Shared.NetMessages.GroupMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class GroupResponse : INetMessage, INetError
    {
        public List<DbGroup> Groups;
        public List<ErrorMessage> ErrorMessages { get;set; }
    }
}
