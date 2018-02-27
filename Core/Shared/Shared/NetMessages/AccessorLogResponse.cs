using Shared.NetMessages.AccessorMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    public class AccessorLogResponse : INetMessage
    {
        /// <summary>
        /// Logy pokud byli odeslány
        /// </summary>
        /// Jsou odeslány pouze pokud se uživatel zeptá na logy
        List<DbAccessorLog> AccessorLogs;
        List<ErrorMessage> ErrorMessages;
    }
}
