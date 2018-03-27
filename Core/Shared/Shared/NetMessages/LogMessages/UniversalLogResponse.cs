using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.LogMessages
{
    public class UniversalLogResponse : INetMessage,INetError
    {
        public IEnumerable<UniversalLogMessage> Logs { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}
