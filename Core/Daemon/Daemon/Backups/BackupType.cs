using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Backups
{
    public class BackupType
    {
        public readonly int ID;

        private BackupType(int i)
        {
            this.ID = i;
        }

        public static BackupType Parse(DbBackupType backupType)
        {
            switch (backupType.Id)
            {
                case 1:
                    return NORM;
                case 2:
                    return DIFF;
                case 3:
                    return INCR;
                default:
                    throw new ArgumentException(backupType.ToString());
            }
        }

        public static readonly BackupType NORM = new BackupType(1);
        public static readonly BackupType DIFF = new BackupType(2);
        public static readonly BackupType INCR = new BackupType(3);
    }
}
