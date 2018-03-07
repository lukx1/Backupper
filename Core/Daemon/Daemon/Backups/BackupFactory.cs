using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Backups
{
    public class BackupFactory
    {
        public BackupType BackupType;
        public string DestinationPath;
        public string SourcePath;
        public bool IsZip;
        public int ID;

        public IBackup CreateFromBackupType()
        {
            if (BackupType == BackupType.NORM)
                return CreateFullBackup();
            else
                throw new NotImplementedException();
        }

        public FullBackup CreateFullBackup()
        {
            FullBackup fullBackup = new FullBackup(DestinationPath, SourcePath);
            return fullBackup;
        }

    }
}
