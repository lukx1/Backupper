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
    public class BackupType
    {
        public static readonly BackupType NORM = new BackupType(1,"NORM","Normal");
        public static readonly BackupType DIFF = new BackupType(2,"DIFF","Differential");
        public static readonly BackupType INCR = new BackupType(3,"INCR","Incremental");

        [DeserializeOnly]
        public BackupType() { }

        private BackupType(int id, string shortName, string longName)
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
