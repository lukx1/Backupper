using DaemonShared.Pipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class LocalException : Exception
    {
        public PipePopup Popup { get; set; } = new PipePopup
        {
            T = "Nečekaná chyba",
            B = PipePopup.Buttons.OK,
            C = "Backupper",
            I = PipePopup.Icons.Error
        };

        public LocalException()
        {
            Popup.T = "\r\n" + this.StackTrace;
        }

        public LocalException(string message) : base(message)
        {
            Popup.T += "\r\n" + message + "\r\n" + this.StackTrace;
        }

        private void ExLoop(StringBuilder builder,Exception e)
        {
            builder.Append(e.Message).Append("\r\n").Append(e.StackTrace).Append("\r\n");
            if (e.InnerException != null)
                ExLoop(builder, e.InnerException);
        }

        public LocalException(string message, Exception innerException) : base(message, innerException)
        {
            StringBuilder builder = new StringBuilder();
            ExLoop(builder, this);
            this.Popup.T = builder.ToString();
        }

        protected LocalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
