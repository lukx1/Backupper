using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    /// <summary>
    /// Pro vytváření, upravování a odstraňování tasků 
    /// </summary>
    /// 
    ///     Vytváření:
    ///         Vše až na uuidDaemon
    ///         
    ///     Odstranení:
    ///         Pouze uuidDaemon
    ///         
    ///     Upravování:
    ///         Vše
    ///         
    public class DbTask
    {
        public int id;
        /// <summary>
        /// Uuid daemona, pokud odesláno daemonem bude odvozeno z sessionUuid
        /// </summary>
        public Guid uuidDaemon;
        
        /// <summary>
        /// Jméno tasku
        /// </summary>
        public string name;

        /// <summary>
        /// Popis tasku
        /// </summary>
        public string description;

        /// <summary>
        /// Lokace která mají být zkopírovany a kam kopírovat
        /// </summary>
        public List<DbTaskLocation> taskLocations;

        /// <summary>
        /// Kdy má task proběhnout
        /// </summary>
        public DbTime time;

        /// <summary>
        /// Naposledy změněno
        /// </summary>
        public DateTime lastChanged;
    }
}
