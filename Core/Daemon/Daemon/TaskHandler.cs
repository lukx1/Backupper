using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Daemon.Backups;
using System.Configuration;
using Daemon.Logging;
using Shared;

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
        private ILogger logger = LoggerFactory.CreateAppropriate();
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
        /// Vypočitá kdy by měla proběhnout záloha
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private TimeSpan CalculateDueTime(DateTime startTime,int? period) //TODO: kontrola jestli minuly backup probeh
        {
            if (period == null || period <= 0)
                return TimeSpan.FromMilliseconds(0);
            int res = (int)period - ((int)(DateTime.Now - startTime).TotalMilliseconds%(int)period);
            return TimeSpan.FromMilliseconds(res);
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
            timedBackup.Backup = CreateBackupInstance(taskLocation,taskLocation.id*time.id);
            var dueTime = CalculateDueTime(time.startTime, time.interval);
            if (dueTime.Milliseconds != 0)
                logger.Log($"Záloha proběhne za {dueTime}",LogType.DEBUG);
            timedBackup.Timer = new Timer((e) => // Sestaví timer
            { //Tělo timeru

                if (!timedBackup.ShouldRun.Value) // Kontroluje jestli by měl běžet
                {
                    timedBackup.Dispose(); // Zničí timer
                    return;
                }
                if(time.startTime < DateTime.Now && !time.repeat)
                {
                    logger.Log($"{time.id * idTask}:Timer měl proběhnout v minulosti a neměl se opakovat, bude zničen", LogType.DEBUG);
                    timedBackup.ShouldRun.Value = false;
                    timedBackup.Dispose();
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
                    //timedBackup.Backup.StartBackup(); TODO - Na Backup class, ne na IBackup 
                    timedBackup.IsRunning.Value = false;
                    timedBackup.BackupEnded();
                    if (!time.repeat)
                    {
                        logger.Log($"{time.id * idTask}:Timer zálohoval a nemá se opakovat, bude zničen", LogType.DEBUG);
                        timedBackup.ShouldRun.Value = false;
                        timedBackup.Dispose();
                        return;
                    }
                }

            }, null, dueTime, TimeSpan.FromMilliseconds(time.interval));
            return timedBackup;
        }

        /// <summary>
        /// Odstraní timery které doběhly a u běžících čeká než doběhnout a pak je odstraní
        /// </summary>
        private void ClearTObjs()
        {
            logger.Log("Čištení tObj...", LogType.DEBUG);
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
            logger.Log("Čištení tObj OK",LogType.DEBUG);
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
                        logger.Log($"Vytvořen timer pro task #{task.id}, taskLoc #{taskLocation.id}, time #{time.id}{Util.Newline}Čas start:{time.startTime}, interval:{time.interval}s,opakovat:{time.repeat}, konec:{time.endTime}",LogType.DEBUG);
                        tBackups.Add(CreateTimedBackup(taskLocation, time,task.id));
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
        private IBackup CreateBackupInstance(DbTaskLocation taskLocation, int id = -1)
        {
            backupFactory.ID = id == -1 ? taskLocation.id : id;
            backupFactory.BackupType = BackupType.Parse(taskLocation.backupType);
            backupFactory.DestinationPath = taskLocation.destination.uri;
            backupFactory.SourcePath = taskLocation.source.uri;
            backupFactory.IsZip = taskLocation.taskLocationDetails != null; //TODO : Pridat TaskLocation details do db

            return backupFactory.CreateFromBackupType();
        }
        
        /// <summary>
        /// Debugovací metoda
        /// </summary>
        /// <param name="backup"></param>
        private void DebugMethod(IBackup backup)
        {
            logger.Log($"{backup.ID}: {backup.SourcePath} -> {backup.DestinationPath} (ZIP={backup.ShouldZip})",LogType.DEBUG);
        }

    }
}
