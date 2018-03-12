using Daemon.Backups;
using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daemon
{
    public class TimedBackup : IDisposable
    {//TODO : event?

        public int IdTask { get; set; }
        public DbTaskLocation TaskLocation { get; set; }
        public Timer Timer { get; set; }
        public IBackup Backup { get; set; }

        /// <summary>
        /// Zničí tento objekt. Objekt může být zničen pouze pokud neběží zálohování
        /// </summary>
        public void Dispose()
        {
            if (IsRunning.Value)
                throw new InvalidOperationException("IsRunning = true");
            Timer.Dispose();
            TaskLocation = null;
        }

        

        public class BoolWrapper
        {
            public bool Value { get; set; }
            public BoolWrapper(bool value) { this.Value = value; }
        }

        /// <summary>
        /// Pokud je nastaveno na false, bude tento objekt zničen při
        /// příštím průběhu timeru
        /// </summary>
        public readonly BoolWrapper ShouldRun = new BoolWrapper(true);
        /// <summary>
        /// Značí pokud probíhá zálohování tímto objektem
        /// </summary>
        public readonly BoolWrapper IsRunning = new BoolWrapper(false);
    }
}
