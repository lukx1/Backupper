﻿using Daemon.Backups;
using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daemon
{
    /// <summary>
    /// Provádí zálohu v daný čas
    /// </summary>
    public class TimedBackup : IDisposable
    {

        /// <summary>
        /// Pro identifikace zdroje
        /// </summary>
        public int IdTask { get; set; }

        /// <summary>
        /// Časovač
        /// </summary>
        /// Nedoporučuje se na timeru ručně volat Dispose
        public Timer Timer { get; set; }
        /// <summary>
        /// Zálohovač
        /// </summary>
        /// Nedoporučuje se na timeru ručně volat StartBackup
        public IBackup Backup { get; set; }

        /// <summary>
        /// Zničí tento objekt. Objekt může být zničen pouze pokud neběží zálohování
        /// </summary>
        public void Dispose()
        {
            if (Timer == null)
                return;
            if (IsRunning.Value)
                throw new InvalidOperationException("IsRunning = true");
            Timer.Dispose();
        }

        
        /// <summary>
        /// Bool reference
        /// </summary>
        public class BoolWrapper
        {
            public bool Value { get; set; }
            public BoolWrapper(bool value) { this.Value = value; }
        }

        /// <summary>
        /// Delegát změny stavu běhu
        /// </summary>
        public delegate void RunChange();
    
        /// <summary>
        /// Zálohování začlo
        /// </summary>
        //public event RunChange BackupStarted;

        /// <summary>
        /// Zálohování skončilo
        /// </summary>
        //public event RunChange BackupEnded;

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
