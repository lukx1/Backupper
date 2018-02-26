using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class TaskLocation
    {
        /// <summary>
        /// Id lokace shodující se s id serveru, při vytváření nemusí být nastaveno
        /// </summary>
        public int id;

        /// <summary>
        /// Odkud kopírovat
        /// </summary>
        public Location source;

        /// <summary>
        /// Kam kopírovat
        /// </summary>
        public Location destination;

        /// <summary>
        /// Jakým způsobem kopírovat
        /// </summary>
        public BackupType backupType;

        /// <summary>
        /// Kdy kopírovat
        /// </summary>
        public List<Time> times;
    }
}
