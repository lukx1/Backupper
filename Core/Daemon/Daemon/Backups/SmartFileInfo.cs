using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Backups
{
    public class SmartFileInfo
    {
        public string filename { get; set; }
        public string destination { get; set; }
        public DateTime lastDateModified { get; set; }
    }
}
