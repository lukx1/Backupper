using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Shared.NetMessages.TaskMessages;
using System.IO;
using Daemon.Communication;

namespace Daemon.Backups
{
    public class SmartBackup : IBackup
    {
        public DbBackupType BackupType { get; set; }
        public DbTaskDetails TaskDetails { get; set; }
        public IEnumerable<DbTaskLocation> TaskLocations { get; set; }
        public int ID { get; set; }

        public string ActionBefore { get; set; }
        public string ActionAfter { get; set; }


        private Logging.ILogger logger = Logging.LoggerFactory.CreateAppropriate();

        public SmartBackup()
        {

        }

        /// <summary>
        /// Začne Backupovat
        /// </summary>
        public void StartBackup()
        {

            CMDAction(ActionBefore);
            logger.Log("Done Action Before >" + ActionBefore, Shared.LogType.INFORMATION);

            foreach (DbTaskLocation item in TaskLocations)
            {
                SmartBackupInfo info = new SmartBackupInfo();
                info.location = item;
                if((item.source.protocol == DbProtocol.WND || item.source.protocol == DbProtocol.WRD)) // Bez tohodle to crashovalo
                    info.CreateFullBackupInfo(item.source.uri);
                if (BackupType.Id == DbBackupType.DIFF.Id)
                {
                    SmartBackupInfo temp = new SmartBackupInfo() { location = item };
                    temp.ReadOldestSimilar();
                    info.Differentiate(temp);
                }
                else if (BackupType.Id == DbBackupType.INCR.Id)
                {
                    info.UnionAllSimilarInfos();
                }
                logger.Log("Done Creating BackupInfo Type=" + BackupType.LongName + " Path=" + info.location,Shared.LogType.INFORMATION);

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
                if (item.source.protocol == DbProtocol.MYSQL)
                {
                    SmartBackupInfo tempInfo = new SmartBackupInfo();

                    var sdest = Path.Combine(item.destination.uri, item.source.uri + ".sql");
                    SqlCommunicator sqlCommunicator = new SqlCommunicator();
                    sqlCommunicator.ExportAsFileAsync(// Od sud
                        item.source.LocationCredential.host,
                        item.source.uri,
                        item.source.LocationCredential.username,
                        item.source.LocationCredential.password,//Az sem nesahat tohle je OK
                        sdest // Kde se soubor vytvori
                        ).Wait();

                    BackupNormal(info, item);//Tohle nevim co je
                }
                logger.Log("Done Backuping using " + item.destination.protocol,Shared.LogType.INFORMATION);
            }
            CMDAction(ActionAfter);
            logger.Log("Done Action After >" + ActionAfter,Shared.LogType.INFORMATION);

        }

        private void BackupMySQL()
        {

        }

        /// <summary>
        /// Spustí CMD Akci
        /// </summary>
        /// <param name="script"></param>
        public void CMDAction(string script)
        {
            if (script == null)
                return;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + script;
            process.StartInfo = startInfo;
            process.Start();
        }

        /// <summary>
        /// Zálohuje soubory na disk či síťový disk
        /// </summary>
        /// <param name="backupInfo"></param>
        /// <param name="taskLocation"></param>
        private void BackupNormal(SmartBackupInfo backupInfo, DbTaskLocation taskLocation)
        {
            SmartBackupInfo trulyBackupedInfo = backupInfo;

            if (TaskDetails.ZipAlgorithm == "zip")
            {
                trulyBackupedInfo = new SmartBackupInfo();
                trulyBackupedInfo = new Compressions.Compressor(backupInfo, TaskDetails).Compress(Path.GetTempPath() + @"\" + DateTime.Now.ToFileTimeUtc() + ".zip",System.IO.Compression.CompressionLevel.Optimal);
                logger.Log("Done compressing file using zip",Shared.LogType.INFORMATION);
            }

            bool successful = true;

            string DestinationPath = taskLocation.destination.uri + $"/{taskLocation.id}_{DateTime.Now.ToFileTimeUtc()}";
            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            string SourcePath = taskLocation.source.uri;
            foreach (SmartFileInfo item in trulyBackupedInfo.fileInfos)
            {
                // Definice cesty kam se to bude kopírovat je = DestinationPath + SubPath + FileName, a kopiruje se z SourcePath + SubPath + FileName (aneb item.destination)
                string subPath = item.destination.Substring(SourcePath.Length, item.destination.Length - SourcePath.Length - item.filename.Length);
                if (!Directory.Exists(DestinationPath + subPath))
                    Directory.CreateDirectory(DestinationPath + subPath);
                string copyPath = DestinationPath + subPath + item.filename;
                try
                {
                    logger.Log("Backuped file " + item.destination + " to " + copyPath,Shared.LogType.INFORMATION);
                    File.Copy(item.destination, copyPath);
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

       

        /// <summary>
        /// Zálohuje pomocí FTP
        /// </summary>
        /// <param name="backupInfo"></param>
        /// <param name="taskLocation"></param>
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
                    logger.Log("Backuped file " + item.destination + " to " + copyPath,Shared.LogType.INFORMATION);
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

        /// <summary>
        /// Zálohuje pomocí SFTP
        /// </summary>
        /// <param name="backupInfo"></param>
        /// <param name="taskLocation"></param>
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
                    logger.Log("Backuped file " + item.destination + " to " + copyPath,Shared.LogType.INFORMATION);
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
