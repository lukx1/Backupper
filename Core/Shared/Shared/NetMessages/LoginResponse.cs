using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class LoginResponse : INetMessage
    {
        public Guid sessionUuid;
        public ErrorMessage errorMessage;
    }
}
