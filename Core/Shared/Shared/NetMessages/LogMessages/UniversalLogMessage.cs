using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.LogMessages
{
    public class UniversalLogMessage : SessionMessage, INetError
    {
        public IEnumerable<JsonableUniversalLog> Logs { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}
