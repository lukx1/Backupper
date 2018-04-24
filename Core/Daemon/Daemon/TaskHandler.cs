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
using Shared.LogObjects;
using DaemonShared;

namespace Daemon
{
    /// <summary>
    /// Stará se o průběh tasků
    /// </summary>
    public class TaskHandler : IDisposable
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
            return false;
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
            double res = /*(double)period - */Math.Abs(((((double)(DateTime.Now - startTime).TotalSeconds)%(double)period)));
            return TimeSpan.FromSeconds(res);
        }

        /// <summary>
        /// Metoda kterou timer volá
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time"></param>
        /// <param name="timedBackup"></param>
        private void TimerMethod(DbTask task, DbTime time, TimedBackup timedBackup)
        {
            if (!timedBackup.ShouldRun.Value) // Kontroluje jestli by měl běžet
            {
                timedBackup.Dispose(); // Zničí timer
                return;
            }
            if (time.startTime.AddMinutes(5) < DateTime.Now && !time.repeat)/*5 minut jako padding casu*/
            {
                logger.Log($"{time.id * task.id}:Timer měl proběhnout v minulosti a neměl se opakovat, bude zničen", LogType.DEBUG);
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
                //timedBackup.BackupStarted();
                timedBackup.IsRunning.Value = true;
                logger.Log($"Nyní probíhá zálohování {timedBackup.Backup.ID}", LogType.INFORMATION);
                try
                {
                    timedBackup.Backup.StartBackup();
                    logger.Log($"Zálohování {timedBackup.Backup.ID} dokončeno", LogType.INFORMATION);
                }
                catch (Exception ex)
                {
                    logger.Log("Neočekávaná chyba při zálohování", LogType.ERROR);
                    Exception ex2 = ex;
                    while(ex2 != null)
                    {
                        logger.Log($"Exception {ex2}-{ex2.Message}{Environment.NewLine}Stack Trace{ex2.StackTrace}",LogType.ERROR);
                        ex2 = ex2.InnerException;
                    }
                    Task.Run(async () => await logger.ServerLogAsync(new GeneralTaskFailedLog(task.id, ex, time)));
                }
                timedBackup.IsRunning.Value = false;
                //timedBackup.BackupEnded();
                if (!time.repeat)
                {
                    logger.Log($"{time.id * task.id}:Timer zálohoval a nemá se opakovat, bude zničen", LogType.DEBUG);
                    timedBackup.ShouldRun.Value = false;
                    timedBackup.Dispose();
                    return;
                }
            }
        }

        /// <summary>
        /// Vytvoří jeden objekt zálohování, na každý čas je vytvořen jeden objekt
        /// </summary>
        /// Vícero TimedBackapů může mít identický taskLocation
        /// <param name="taskLocation">Jak záloha má probýhat</param>
        /// <param name="time">Kdy má proběhnout</param>
        /// <param name="idTask">Jakému tasku patří</param>
        /// <returns></returns>
        private TimedBackup CreateTimedBackup(DbTask task,DbTime time) //TODO: Reformatovat
        {
            TimedBackup timedBackup = new TimedBackup // Nastaví zálkatdní hodnoty
            {
                IdTask = task.id,
            };
            
            timedBackup.Backup = CreateBackupInstance(task.taskLocations,task.backupType,task.details,task.ActionBefore,task.ActionAfter,task.id*time.id);
            var dueTime = CalculateDueTime(time.startTime, time.interval);
            if (dueTime.Milliseconds != 0)
                logger.Log($"Záloha proběhne za {dueTime}",LogType.DEBUG);
            timedBackup.Timer = new Timer((e) => // Sestaví timer
            { //Tělo timeru
                TimerMethod(task,time,timedBackup);
            }, null, dueTime, TimeSpan.FromSeconds(time.interval));
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

        private void ReshapeToTestingTime(DbTime time)
        {
            var ttime = CreateTestingTime();
            time.endTime = ttime.endTime;
            time.id = ttime.id;
            time.interval = ttime.interval;
            time.name = ttime.name;
            time.repeat = ttime.repeat;
            time.startTime = ttime.startTime;
        }

        private DbTime CreateTestingTime()
        {
            return new DbTime() {endTime = null,id = 999,interval = 300,name = "Test time",repeat = false,startTime = DateTime.Now.AddSeconds(5) };
        }

        /// <summary>
        /// z listu tasků vytvoří timery
        /// </summary>
        public void CreateTimers()
        {

            ClearTObjs();
            foreach (var task in Tasks)
            {
                if(task.times.Count < 1)
                {
                    logger.Log($"Byl přijat task({task.name}) bez času, nikdy neproběhne",LogType.WARNING);
                    var faf = logger.ServerLogAsync(new GeneralTaskFailedLog(task.id, new ArgumentNullException("Nebyl nastaven čas, kdy má úloha běžet"),null) { LogType = LogType.WARNING});
                }
                foreach (var time in task.times)
                {
                    if(new LoginSettings().TimerDebugOnly)
                        ReshapeToTestingTime(time);
                    logger.Log($"Vytvořen timer pro task #{task.id},time #{time.id}{Environment.NewLine}Čas start:{time.startTime}, interval:{time.interval}s,opakovat:{time.repeat}, konec:{(time.endTime == null ? "Nikdy":time.endTime.ToString())}", LogType.DEBUG);
                    tBackups.Add(CreateTimedBackup(task, time));
                }
            }
        }

        //TODO : Finish
        /// <summary>
        /// Vyvoří IBackup z informací v tasklocationu
        /// </summary>
        /// <param name="taskLocation">Jak zálohovat</param>
        /// <returns>IBackup</returns>
        private IBackup CreateBackupInstance(IEnumerable<DbTaskLocation> taskLocations,DbBackupType backupType, DbTaskDetails taskDetails, string actionBefore,string actionAfter, int id = -1)
        {
            return BackupFactory.CreateBackup(taskLocations, backupType, taskDetails,id,actionBefore,actionAfter);
        }
        
        /// <summary>
        /// Debugovací metoda
        /// </summary>
        /// <param name="backup"></param>
        private void DebugMethod(IBackup backup)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(backup.ID).Append(" :").Append(Environment.NewLine);
            foreach (var loc in backup.TaskLocations)
            {
                builder.Append($"{loc.source} -> {loc.destination} (ZIP={backup.TaskDetails.ZipAlgorithm != null}){Environment.NewLine}");
            }
            logger.Log(builder.ToString(),LogType.DEBUG);
        }

        public void Dispose()
        {
            tBackups.ForEach(r =>  r.Dispose());
            notGarbage.ForEach(r => r.Dispose());
        }
    }
}
