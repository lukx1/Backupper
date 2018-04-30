using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class UserLoginResponse : INetMessage, INetError
    {
        public bool OK { get; set; }
        public string PrivateKeyEncrypted { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}
