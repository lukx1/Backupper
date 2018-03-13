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
        public int ID { get; set; }
        List<IBackup> AllBackups { get; set; }


        public Backup(int id)
        {
            AllBackups = new List<IBackup>();
            this.ID = id;
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
                foreach (IBackup item in AllBackups)
                    item.StartBackup(pathItem + "/" + ID + "/" + item.ID + "/Backup");
            }
          
        }

        public void AddBackup(IBackup backup)
        {
            backup.ID = AllBackups.Count;
            backup.DestinationPath = "";
            AllBackups.Add(backup);
        }
    }
}
