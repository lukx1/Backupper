using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    /// <summary>
    /// Jeden 
    /// </summary>
    public class DbTask
    {   
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
        public List<TaskLocation> taskLocations;
    }
}
