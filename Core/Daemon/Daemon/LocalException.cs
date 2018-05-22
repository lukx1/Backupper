using DaemonShared.Pipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    /// <summary>
    /// Vyjímka která by neměla či nemůže být odeslána serveru
    /// a má být uložena na lokálním počítači
    /// </summary>
    public class LocalException : Exception
    {
        /// <summary>
        /// Co by mělo vyskočit na obrazovce, pokud je DS
        /// podporován
        /// </summary>
        public PipePopup Popup { get; set; } = new PipePopup
        {
            ///Text
            T = "Nečekaná chyba",
            ///Tlačitko
            B = PipePopup.Buttons.OK,
            ///Hlavička
            C = "Backupper",
            ///Druh ikony
            I = PipePopup.Icons.Error
        };

        /// <summary>
        /// Standardní konstruktor který do Popup
        /// zkopíruje StackTrace
        /// </summary>
        public LocalException()
        {
            Popup.T = "\r\n" + this.StackTrace;
        }

        /// <summary>
        /// Standardní konstruktor který do Popup
        /// zkopíruje StackTrace a zprávu
        /// </summary>
        public LocalException(string message) : base(message)
        {
            Popup.T += "\r\n" + message + "\r\n" + this.StackTrace;
        }

        /// <summary>
        /// Vypíše interní expectiony
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="e"></param>
        private void ExLoop(StringBuilder builder,Exception e)
        {
            builder.Append(e.Message).Append("\r\n").Append(e.StackTrace).Append("\r\n");
            if (e.InnerException != null)
                ExLoop(builder, e.InnerException);
        }

        /// <summary>
        /// Standardní konstruktor který do Popup
        /// zkopíruje StackTrace a vnitřní exception
        /// </summary>
        public LocalException(string message, Exception innerException) : base(message, innerException)
        {
            StringBuilder builder = new StringBuilder();
            ExLoop(builder, this);
            this.Popup.T = builder.ToString();
        }

        /// <summary>
        /// Speciální konstruktor, je nutno manuálně nastavit Popup
        /// </summary>
        protected LocalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
