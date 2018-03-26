using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ILogHeader
    {
        /// <summary>
        /// Hash určující laké části logu k sobě patří. Pokud je pages 1, tak nemusí být nastaven
        /// </summary>
        string Binder { get; set; }
        /// <summary>
        /// Jaká stránka toto je
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Kolik stránek celkem je
        /// </summary>
        int Pages { get; set; }
    }
}
