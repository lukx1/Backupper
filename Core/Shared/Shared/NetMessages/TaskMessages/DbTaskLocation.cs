using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class DbTaskLocation
    {
        /// <summary>
        /// Id lokace shodující se s id serveru, při vytváření nemusí být nastaveno
        /// </summary>
        public int id;

        /// <summary>
        /// Odkud kopírovat
        /// </summary>
        public DbLocation source;

        /// <summary>
        /// Kam kopírovat
        /// </summary>
        public DbLocation destination;

        /// <summary>
        /// Jakým způsobem kopírovat
        /// </summary>
        public DbBackupType backupType;

        /// <summary>
        /// Detaily o tom jak má zálohování proběhnout
        /// </summary>
        public DbTaskDetails taskLocationDetails;

    }
}
