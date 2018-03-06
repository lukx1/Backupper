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
        public static string BackupMainDirectory = "C:/Users/rambo_000/Desktop/TESTFOLDER/BACKUPS/";
        public int ID { get; set; }
        List<IBackup> AllBackups { get; set; }

        public Backup()
        {
            AllBackups = new List<IBackup>();
        }

        public void BackupAll()
        {
            if (!Directory.Exists(BackupMainDirectory + ID))
                Directory.CreateDirectory(BackupMainDirectory + ID);
            foreach (IBackup item in AllBackups)
                item.StartBackup();
        }

        public void AddBackup(IBackup backup)
        {
            backup.ID = AllBackups.Count;
            backup.destinationPath = BackupMainDirectory + ID + "/" + backup.ID + "/Backup";
            AllBackups.Add(backup);
        }
    }
}
