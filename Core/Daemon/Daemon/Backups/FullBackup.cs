using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Shared.NetMessages.TaskMessages;

namespace Daemon.Backups
{
    public class FullBackup : IBackup
    {
        public string DestinationPath { get; set; }
        public string SourcePath { get; set; }
        public bool ShouldZip { get; set; }
        public int ID { get; set; }
        public BackupInfo backupInfo { get; set; }
        public IEnumerable<DbTaskLocation> TaskLocations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbBackupType BackupType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbTaskDetails TaskDetails { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cilova Destinace"></param>
        /// <param name="Destinace Zalohovanych souboru"></param>

        public FullBackup(string sourcePath, bool shouldZip = false)
        {
            this.SourcePath = sourcePath;
            ShouldZip = shouldZip;
            backupInfo = new BackupInfo(this);
        }

        void Backup(DirectoryInfo dir, string Destination)
        {
            if (!Directory.Exists(Destination))
                Directory.CreateDirectory(Destination);
            foreach (FileInfo item in dir.GetFiles())
                File.Copy(item.FullName, Destination + "/" + item.Name);
            foreach (DirectoryInfo item in dir.GetDirectories())
            {
                Directory.CreateDirectory(Destination + "/" + item.Name);
                Backup(item, Destination + "/" + item.Name);
            }
        }

        void ZipBackup(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            ZipFile.CreateFromDirectory(SourcePath, path + "/Backup.zip");
        }

        public void StartBackup(string path)
        {
            if (ShouldZip)
                ZipBackup(path);
            else
                Backup(new DirectoryInfo(SourcePath), path);
            backupInfo.CreateFile(path);
        }

        public void StartBackup()
        {
            throw new NotImplementedException();
        }
    }
}
