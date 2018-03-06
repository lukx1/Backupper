using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Daemon.Backups;
using System.Configuration;

namespace Daemon
{
    public class TaskHandler
    {
        public List<DbTask> Tasks { get; set; }

        private List<Timer> timers = new List<Timer>();
        private BackupFactory backupFactory = new BackupFactory();

        private Timer CreateTimer(DbTaskLocation taskLocation,DbTime time)
        {
            return new Timer((e) =>
            {
                if (ConfigurationManager.AppSettings["TimerDontBackup"] == "True" || ConfigurationManager.AppSettings["TimerDontBackup"] == null)
                {
                    PlaceHolderMethod();
                    return;
                }
                var backupInstance = CreateBackupInstance(taskLocation);
                backupInstance.StartBackup();
            }, null, time.startTime.TimeOfDay, new TimeSpan(0, 0, 0, (int)time.interval, 0));
        }

        public void CreateTimers(bool clear = true)
        {
            timers.Clear();
            foreach (var task in Tasks)
            {
                foreach (var taskLocation in task.taskLocations)
                {
                    foreach (var time in taskLocation.times)
                    {
                        
                    }
                }
                
            }
            
        }

        //TODO : Finish
        private IBackup CreateBackupInstance(DbTaskLocation taskLocation)
        {
            backupFactory.ID = taskLocation.id;
            backupFactory.BackupType = BackupType.Parse(taskLocation.backupType);
            backupFactory.DestinationPath = taskLocation.destination.uri;
            backupFactory.SourcePath = taskLocation.source.uri;
            backupFactory.IsZip = false; //TODO : Pridat TaskLocation details do db

            return backupFactory.CreateFromBackupType();
        }
        //TODO : Finish
        private void PlaceHolderMethod()
        {
            Console.WriteLine(DateTime.Now+": PLACE HOLDER 11234786321");
        }

    }
}
