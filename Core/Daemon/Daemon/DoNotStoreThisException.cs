using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    /// <summary>
    /// Obaluje normální exception a značí, že by neměl být ukládán lokálně
    /// ani odesílán na server
    /// </summary>
    public class DoNotStoreThisExceptionException : Exception
    {
        public DoNotStoreThisExceptionException()
        {
        }

        public DoNotStoreThisExceptionException(string message) : base(message)
        {
        }

        public DoNotStoreThisExceptionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoNotStoreThisExceptionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
