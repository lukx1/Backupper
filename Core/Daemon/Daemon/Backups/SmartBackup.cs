using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Shared.NetMessages.TaskMessages;
using System.IO;

namespace Daemon.Backups
{
    public class SmartBackup : IBackup
    {
        public DbBackupType BackupType { get; set; }
        public DbTaskDetails TaskDetails { get; set; }
        public List<DbTaskLocation> TaskLocations { get; set; }
        public int ID { get; set; }

        public SmartBackup()
        {
            TaskLocations = new List<DbTaskLocation>();
        }

        public void StartBackup()
        {
            foreach (DbTaskLocation item in TaskLocations)
            {
                SmartBackupInfo info = new SmartBackupInfo();
                info.location = item;
                info.CreateFullBackupInfo(item.source.uri);
                if (BackupType == DbBackupType.DIFF)
                {
                    SmartBackupInfo temp = new SmartBackupInfo() { location = item };
                    temp.ReadOldestSimilar();
                    info.Differentiate(temp);
                }
                else if (BackupType == DbBackupType.INCR)
                {
                    info.UnionAllSimilarInfos();
                }
                BackupNormal(info, item);
            }
        }


        /// <summary>
        /// Zálohuje soubory na disk či síťový disk
        /// </summary>
        /// <param name="backupInfo"></param>
        /// <param name="taskLocation"></param>
        private void BackupNormal(SmartBackupInfo backupInfo,DbTaskLocation taskLocation)
        {
            string DestinationPath = taskLocation.destination.uri + $"/{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}";
            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            string SourcePath = taskLocation.source.uri;
            foreach (SmartFileInfo item in backupInfo.fileInfos)
            {
                // Definice cesty kam se to bude kopírovat je = DestinationPath + SubPath + FileName, a kopáruje se z SourcePath + SubPath + FileName (aneb item.destination)
                string subPath = item.destination.Substring(SourcePath.Length, item.destination.Length - SourcePath.Length - item.filename.Length);
                if (!Directory.Exists(DestinationPath + subPath))
                    Directory.CreateDirectory(DestinationPath + subPath);
                string copyPath = DestinationPath + subPath + item.filename;
                File.Copy(item.destination, copyPath);
            }

            backupInfo.WriteToFile(SmartBackupInfo.StorePath + $"{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}.bki");
        }

        private void BackupFTP(SmartBackupInfo backupInfo, DbTaskLocation taskLocation)
        {
            string host = taskLocation.destination.LocationCredential.host;
            string user = taskLocation.destination.LocationCredential.username;
            string pass = taskLocation.destination.LocationCredential.password;


        }

    }
}
