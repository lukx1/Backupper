using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Backups
{
    public interface IBackup
    {
        void StartBackup(string path);
        string DestinationPath { get; set; }
        string SourcePath { get; set; }
        bool ShouldZip { get; set; }
        int ID { get; set; }
    }
}
