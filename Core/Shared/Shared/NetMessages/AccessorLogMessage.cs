using Shared.NetMessages.AccessorMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class AccessorLogMessage : SessionMessage
    {
        public List<DbAccessorLog> Accessors;
    }
}
