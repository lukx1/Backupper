using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Daemon.Backups
{
    public class FullBackup 
    {
        string destinationPath { get; set; }
        string backupPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cilova Destinace"></param>
        /// <param name="Destinace Zalohovanych souboru"></param>
        public FullBackup(string destinationPath, string backupPath)
        {
            this.destinationPath = destinationPath;
            this.backupPath = backupPath;
        }

        void Backup(DirectoryInfo dir,string Destination)
        {
            foreach (FileInfo item in dir.GetFiles())
                File.Copy(item.FullName, Destination + "/" + item.Name);
            foreach (DirectoryInfo item in dir.GetDirectories())
            {
                Directory.CreateDirectory(Destination + "/" + item.Name);
                Backup(item, Destination + "/" + item.Name);
            }
        }

        public void StartBackup()
        {
            Backup(new DirectoryInfo(backupPath), destinationPath);
        }
    }
}
