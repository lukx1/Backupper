using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class PreSharedKeyResponse : INetMessage
    {
        public List<DbPreSharedKey> Keys;
        public List<ErrorMessage> ErrorMessages;
    }
}
