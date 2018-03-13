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
    /// <summary>
    /// Stará se o průběh tasků
    /// </summary>
    public class TaskHandler
    {
        /// <summary>
        /// List tasků které se právě provádějí
        /// </summary>
        public List<DbTask> Tasks = new List<DbTask>();
        /// <summary>
        /// Zamezuje garbage kolekci běžících objektů
        /// </summary>
        private List<TimedBackup> notGarbage = new List<TimedBackup>();
        /// <summary>
        /// Platné obj. zálohování
        /// </summary>
        private List<TimedBackup> tBackups = new List<TimedBackup>();
        /// <summary>
        /// Tvoří objekty IBackup
        /// </summary>
        private BackupFactory backupFactory = new BackupFactory();

        /// <summary>
        /// Zjištujě jestli se timery mají být v debug režimu
        /// </summary>
        /// <returns></returns>
        private bool IsBackupBeingDebuged()
        {
            LoginSettings settings = new LoginSettings();
            return settings.TimerDebugOnly;
        }

        /// <summary>
        /// Vytvoří jeden objekt zálohování, na každý čas je vytvořen jeden objekt
        /// </summary>
        /// Vícero TimedBackapů může mít identický taskLocation
        /// <param name="taskLocation">Jak záloha má probýhat</param>
        /// <param name="time">Kdy má proběhnout</param>
        /// <param name="idTask">Jakému tasku patří</param>
        /// <returns></returns>
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
                    timedBackup.BackupStarted();
                    timedBackup.IsRunning.Value = true;
                    timedBackup.Backup.StartBackup();
                    timedBackup.IsRunning.Value = false;
                    timedBackup.BackupEnded();
                }

            }, null, time.startTime.TimeOfDay/*Převádí DateTime na TimeSpan*/, new TimeSpan(0, 0, 0, (int)time.interval, 0)/*viz. pred.*/);
            return timedBackup;
        }

        /// <summary>
        /// Odstraní timery které doběhly a u běžících čeká než doběhnout a pak je odstraní
        /// </summary>
        private void ClearTObjs()
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

        /// <summary>
        /// z listu tasků vytvoří timery
        /// </summary>
        public void CreateTimers()
        {
            ClearTObjs();
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
        /// <summary>
        /// Vyvoří IBackup z informací v tasklocationu
        /// </summary>
        /// <param name="taskLocation">Jak zálohovat</param>
        /// <returns>IBackup</returns>
        private IBackup CreateBackupInstance(DbTaskLocation taskLocation)
        {
            backupFactory.ID = taskLocation.id;
            backupFactory.BackupType = BackupType.Parse(taskLocation.backupType);
            backupFactory.DestinationPath = taskLocation.destination.uri;
            backupFactory.SourcePath = taskLocation.source.uri;
            backupFactory.IsZip = false; //TODO : Pridat TaskLocation details do db

            return backupFactory.CreateFromBackupType();
        }
        
        /// <summary>
        /// Debugovací metoda
        /// </summary>
        /// <param name="backup"></param>
        private void DebugMethod(IBackup backup)
        {
            Console.WriteLine($"{DateTime.Now}: DEBUG Backup 11234786321\r\n" +
                $"{backup.ID}: {backup.SourcePath} -> {backup.DestinationPath} (ZIP={backup.ShouldZip})");
        }

    }
}
