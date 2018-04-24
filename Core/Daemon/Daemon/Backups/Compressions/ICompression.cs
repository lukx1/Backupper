using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.NetMessages.TaskMessages;

namespace Daemon.Backups.Compressions
{
    public interface ICompression
    {
        DbTaskDetails taskDetails { get; set; }
        SmartBackupInfo backupInfo { get; set; }

        string Compress();

    }
}
