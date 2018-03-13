using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    /// <summary>
    /// Použito pokuď server vrátil ErrorMessage/e
    /// </summary>
    public class BadResponseException : Exception
    {
        public List<ErrorMessage> ErrorMessages = new List<ErrorMessage>();

        public BadResponseException(string msg, List<ErrorMessage> errors) : base(msg)
        {
            this.ErrorMessages = errors;
        }

        public BadResponseException(string msg, ErrorMessage error) : base(msg)
        {
            this.ErrorMessages.Add(error);
        }
    }
}
