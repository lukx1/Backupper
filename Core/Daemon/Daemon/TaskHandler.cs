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

        private List<TimedBackup> notGarbage = new List<TimedBackup>();
        private List<TimedBackup> tBackups = new List<TimedBackup>();
        private BackupFactory backupFactory = new BackupFactory();

        private bool IsBackupBeingDebuged()
        {
            return ConfigurationManager.AppSettings["TimerDontBackup"] == "True" || ConfigurationManager.AppSettings["TimerDontBackup"] == null;
        }

        private TimedBackup CreateTimedBackup(DbTaskLocation taskLocation,DbTime time, int idTask) //TODO: Reformatovat
        {
            TimedBackup timedBackup = new TimedBackup // Nastaví zálkatdní hodnoty
            {
                IdTask = idTask,
                TaskLocation = taskLocation
            };
            timedBackup.Backup = CreateBackupInstance(taskLocation);
            var timer = new Timer((e) => // Sestaví timer
            { //Tělo timeru

                if (!timedBackup.ShouldRun.Value) // Kontroluje jestli by měl běžet
                {
                    timedBackup.Dispose(); // Zničí timer
                    return;
                }

                if (IsBackupBeingDebuged())// Debug řádek
                {
                    DebugMethod(timedBackup.Backup);
                }
                else // Normální
                {
                    timedBackup.IsRunning.Value = true;
                    timedBackup.Backup.StartBackup();
                    timedBackup.IsRunning.Value = false;
                }

            }, null, time.startTime.TimeOfDay/*Převádí DateTime na TimeSpan*/, new TimeSpan(0, 0, 0, (int)time.interval, 0)/*viz. pred.*/);
            return timedBackup;
        }

        private void ClearTObj()
        {
            notGarbage.RemoveAll(r => // Odstraní všechny objekty které doběhly
            {
                if (!r.IsRunning.Value)
                    return false;
                r.Dispose();
                return true;
            }
            );
            foreach (var t in tBackups)
            {
                t.ShouldRun.Value = false;
                if (!t.IsRunning.Value)
                    t.Dispose();
                else
                {
                    notGarbage.Add(t);
                }
            }
            tBackups.Clear();
        }

        public void CreateTimers()
        {
            ClearTObj();
            foreach (var task in Tasks)
            {
                foreach (var taskLocation in task.taskLocations)
                {
                    foreach (var time in taskLocation.times)
                    {
                        var timedBackup = CreateTimedBackup(taskLocation, time,task.id);
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
        private void DebugMethod(IBackup backup)
        {
            Console.WriteLine(DateTime.Now + ": DEBUG Backup 11234786321\r\n" +
                String.Format("{0}: {1} -> {2} (ZIP={3})",backup.ID,backup.SourcePath,backup.DestinationPath,backup.ShouldZip));
        }

    }
}
