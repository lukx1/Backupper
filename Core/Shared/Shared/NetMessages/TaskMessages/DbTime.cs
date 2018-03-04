using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class DbTime
    {
        /// <summary>
        /// Id času, při vytváření nemusí být nastaveno 
        /// </summary>
        public int id;

        /// <summary>
        /// Interval záloh v sekundách
        /// </summary>
        public int? interval;

        /// <summary>
        /// Jméno času
        /// </summary>
        public string name;

        /// <summary>
        /// Jestli se záloha bude opakovat
        /// </summary>
        public bool repeat;

        /// <summary>
        /// Začátek rozmezí ve kterém se bude zálohovat
        /// </summary>
        public DateTime startTime;

        /// <summary>
        /// Konec rozmezí ve kterém se bude zálohovat
        /// </summary>
        public DateTime? endTime;
    }
}
