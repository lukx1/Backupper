using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Backups
{
    public interface IBackup
    {
        void StartBackup();
        DbBackupType BackupType { get; set; }
        DbTaskDetails TaskDetails { get; set; }
        IEnumerable<DbTaskLocation> TaskLocations { get; set; }
        int ID { get; set; }
    }
}
