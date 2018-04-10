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
        public IEnumerable<DbTaskLocation> TaskLocations { get; set; }
        public int ID { get; set; }

        private Logging.ILogger logger = Logging.LoggerFactory.CreateAppropriate();

        public SmartBackup()
        {
            
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


                if (item.destination.protocol == DbProtocol.FTP)
                {
                    BackupFTP(info, item);
                }
                else if (item.destination.protocol == DbProtocol.SFTP)
                {
                    BackupSFTP(info, item);
                }
                else if (item.destination.protocol == DbProtocol.WND || item.destination.protocol == DbProtocol.WRD)
                {
                    BackupNormal(info, item);
                }
            }
        }


        /// <summary>
        /// Zálohuje soubory na disk či síťový disk
        /// </summary>
        /// <param name="backupInfo"></param>
        /// <param name="taskLocation"></param>
        private void BackupNormal(SmartBackupInfo backupInfo,DbTaskLocation taskLocation)
        {
            bool successful = true;

            string DestinationPath = taskLocation.destination.uri + $"/{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}";
            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            string SourcePath = taskLocation.source.uri;
            foreach (SmartFileInfo item in backupInfo.fileInfos)
            {
                // Definice cesty kam se to bude kopírovat je = DestinationPath + SubPath + FileName, a kopiruje se z SourcePath + SubPath + FileName (aneb item.destination)
                string subPath = item.destination.Substring(SourcePath.Length, item.destination.Length - SourcePath.Length - item.filename.Length);
                if (!Directory.Exists(DestinationPath + subPath))
                    Directory.CreateDirectory(DestinationPath + subPath);
                string copyPath = DestinationPath + subPath + item.filename;
                try
                {
                    File.Copy(item.destination, copyPath);
                }
                catch (Exception)
                {
                    logger.Log($"Backup: Failed to Copy file [Backup: Normal, CopyPath: {copyPath}] backup failed]", Shared.LogType.ERROR);
                    successful = false;
                    break;
                }
            }

            if(successful)
                backupInfo.WriteToFile(SmartBackupInfo.StorePath + $"{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}.bki");
        }

        private void BackupFTP(SmartBackupInfo backupInfo, DbTaskLocation taskLocation)
        {
            Communication.FtpClient client = new Communication.FtpClient(
                taskLocation.destination.LocationCredential.host,
                taskLocation.destination.LocationCredential.username,
                taskLocation.destination.LocationCredential.password
                );

            bool successful = true;

            string DestinationPath = taskLocation.destination.uri + $"/{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}";

            client.createDirectory(DestinationPath);

            string SourcePath = taskLocation.source.uri;
            foreach (SmartFileInfo item in backupInfo.fileInfos)
            {
                // Definice cesty kam se to bude kopírovat je = DestinationPath + SubPath + FileName, a kopiruje se z SourcePath + SubPath + FileName (aneb item.destination)
                string subPath = item.destination.Substring(SourcePath.Length, item.destination.Length - SourcePath.Length - item.filename.Length);
                client.createDirectory(DestinationPath + subPath);
                string copyPath = DestinationPath + subPath + item.filename;
                try
                {
                    client.upload(item.destination, copyPath);
                }
                catch (Exception)
                {
                    logger.Log($"Backup: Failed to Copy file [Backup: Normal, CopyPath: {copyPath}] backup failed]", Shared.LogType.ERROR);
                    successful = false;
                    break;
                }
            }

            if (successful)
                backupInfo.WriteToFile(SmartBackupInfo.StorePath + $"{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}.bki");
        }

        private void BackupSFTP(SmartBackupInfo backupInfo, DbTaskLocation taskLocation)
        {
            Communication.SftpClient client = new Communication.SftpClient(
                taskLocation.destination.LocationCredential.host, 
                taskLocation.destination.LocationCredential.username, 
                taskLocation.destination.LocationCredential.password
                );

            bool successful = true;

            string DestinationPath = taskLocation.destination.uri + $"/{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}";

            client.CreateDirectory(DestinationPath);

            string SourcePath = taskLocation.source.uri;
            foreach (SmartFileInfo item in backupInfo.fileInfos)
            {
                // Definice cesty kam se to bude kopírovat je = DestinationPath + SubPath + FileName, a kopiruje se z SourcePath + SubPath + FileName (aneb item.destination)
                string subPath = item.destination.Substring(SourcePath.Length, item.destination.Length - SourcePath.Length - item.filename.Length);
                client.CreateDirectory(DestinationPath + subPath);
                string copyPath = DestinationPath + subPath + item.filename;
                try
                {
                    client.Upload(item.destination, copyPath);
                }
                catch (Exception)
                {
                    logger.Log($"Backup: Failed to Copy file [Backup: Normal, CopyPath: {copyPath}] backup failed]", Shared.LogType.ERROR);
                    successful = false;
                    break;
                }
            }

            if (successful)
                backupInfo.WriteToFile(SmartBackupInfo.StorePath + $"{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}.bki");
        }

    }
}
