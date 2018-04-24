using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class OneClickResponse : INetError, INetMessage
    {
        public List<ErrorMessage> ErrorMessages { get; set; }
        public Guid uuid { get; set; }
        public string password { get; set; }
    }
}
