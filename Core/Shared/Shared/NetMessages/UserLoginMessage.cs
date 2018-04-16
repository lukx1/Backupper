using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class UserLoginMessage : INetMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
