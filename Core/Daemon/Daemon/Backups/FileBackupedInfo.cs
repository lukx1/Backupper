using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Daemon.Backups
{
    public class FileBackupedInfo
    {
        /// <summary>
        /// Path sub main backup path
        /// </summary>
        public string subRootPath { get; set; }
        public BackupInfo pBackup { get; set; }
        public string name { get; set; }
        public int size { get; set; }

        public FileBackupedInfo()
        {
           
        }
    }
}
