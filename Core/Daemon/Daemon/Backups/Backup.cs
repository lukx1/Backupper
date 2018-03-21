using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Daemon.Backups
{
    
    public class Backup
    {
        List<string> BackupDestinations = new List<string>();
        List<string> BackupSources = new List<string>();
        public int ID { get; set; }
        List<IBackup> AllBackups { get; set; }
        public BackupType backupType { get; set; }
        public bool Zipped = false;


        public Backup(int id,BackupType type)
        {
            AllBackups = new List<IBackup>();
            this.ID = id;
            this.backupType = type;
        }

        public void AddDestination(string destination)
        {
            BackupDestinations.Add(destination);
        }

        public void BackupAll()
        {
            foreach (string pathItem in BackupDestinations)
            {
                if (!Directory.Exists(pathItem + "/"  +  ID))
                    Directory.CreateDirectory(pathItem + "/" + ID);

                for (int i = 0; i < BackupSources.Count; i++)
                {
                    new FullBackup(BackupSources[i], Zipped).StartBackup(pathItem + "/" + ID + "/" + i + "/Backup");
                }
            }
          
        }

        public void AddBackup(IBackup backup)
        {
            backup.ID = AllBackups.Count;
            backup.DestinationPath = "";
            AllBackups.Add(backup);
        }

        public void AddSource(string sourcePath)
        {
            BackupSources.Add(sourcePath);
        }
    }
}
