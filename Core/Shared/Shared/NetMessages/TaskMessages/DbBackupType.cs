using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    /// <summary>
    /// Druh backupu podle mysql databáze
    /// </summary>
    public class DbBackupType
    {
        public static readonly DbBackupType NORM = new DbBackupType(1,"NORM","Normal");
        public static readonly DbBackupType DIFF = new DbBackupType(2,"DIFF","Differential");
        public static readonly DbBackupType INCR = new DbBackupType(3,"INCR","Incremental");

        [DeserializeOnly]
        public DbBackupType() { }

        private DbBackupType(int id, string shortName, string longName)
        {
            Id = id;
            ShortName = shortName;
            LongName = longName;
        }

        [DeserializeOnly]
        public int Id;
        [DeserializeOnly]
        public string ShortName;
        [DeserializeOnly]
        public string LongName;

    }
}
