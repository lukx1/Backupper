using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.NetMessages.TaskMessages;
using System.IO;

namespace Daemon.Backups
{
    public class DetailedLog
    {
        public DbBackupType BackupType { get; set; }
        public DbTaskDetails TaskDetails { get; set; }
        public int ID { get; set; }
        private List<string> logs = new List<string>();

        public DetailedLog(DbBackupType type,DbTaskDetails details, int id)
        {
            BackupType = type;
            TaskDetails = details;
            ID = id;
        }

        /// <summary>
        /// Add new Log entry
        /// </summary>
        /// <param name="s"></param>
        public void Add(string s)
        {
            logs.Add($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}.{DateTime.Now.Millisecond}] " + s);
        }

        /// <summary>
        /// Saves log to file
        /// </summary>
        public void Save()
        {
            if (!Directory.Exists(Path.Combine(Shared.Util.GetAppdataFolder(), "DetailedBackupLogs")))
                Directory.CreateDirectory(Path.Combine(Shared.Util.GetAppdataFolder(), "DetailedBackupLogs"));
            using (StreamWriter writer = new StreamWriter(Path.Combine(Shared.Util.GetAppdataFolder(), "DetailedBackupLogs",$"{DateTime.Now.ToFileTimeUtc()}_{ID}.txt")))
            {
                writer.WriteLine($"BackupType: {BackupType.LongName}");
                writer.WriteLine($"Compression: {TaskDetails.ZipAlgorithm}");
                writer.WriteLine($"CompressionLevel: {TaskDetails.CompressionLevel}");
                foreach (string item in logs)
                {
                    writer.WriteLine(item);
                }
            }
        }
    }
}
