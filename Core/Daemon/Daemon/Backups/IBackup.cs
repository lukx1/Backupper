using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Backups
{
    public interface IBackup
    {
        void StartBackup();
        string destinationPath { get; set; }
        string backupPath { get; set; }
        bool zip { get; set; }
        int ID { get; set; }
    }
}
