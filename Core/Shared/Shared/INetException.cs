using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class INetException<T> : Exception
    {

        public Messenger.ServerMessage<T> ServerResponse { get; private set; }

        public List<ErrorMessage> ErrorMessages = new List<ErrorMessage>();

        public INetException(Messenger.ServerMessage<T> serverResponse, string msg, List<ErrorMessage> errors) : base(msg)
        {
            this.ServerResponse = serverResponse;
            this.ErrorMessages = errors;
        }

        public INetException(Messenger.ServerMessage<T> serverResponse, string msg, ErrorMessage error) : base(msg)
        {
            this.ServerResponse = serverResponse;
            this.ErrorMessages.Add(error);
        }
    }
}
