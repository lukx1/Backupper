using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.LogMessages
{
    public class SpecificLogResponse : INetMessage,INetError
    {
        public IEnumerable<JsonableSpecificLog> Logs { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}
