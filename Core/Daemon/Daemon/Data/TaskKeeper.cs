using Daemon.Logging;
using DaemonShared;
using Shared;
using Shared.LogObjects;
using Shared.NetMessages;
using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Data
{
    /// <summary>
    /// Tvoří lokální záznamy o taskách
    /// </summary>
    public class TaskKeeper
    {
        private Messenger messenger;
        private readonly static string StoreDir = Path.Combine(Util.GetAppdataFolder(), "Tasks");
        private ILogger logger = LoggerFactory.CreateAppropriate();

        /// <summary>
        /// Instance taskKeeper s messengerem pro komunikaci se serverem
        /// </summary>
        /// <param name="messenger">Pro komunikaci</param>
        public TaskKeeper(Messenger messenger)
        {
            Directory.CreateDirectory(StoreDir);
            this.messenger = messenger;
        }

        /// <summary>
        /// Fetchuje tasky z db
        /// </summary>
        /// <returns>List tasků nebo prázdný list</returns>
        private async Task<List<DbTask>> GetAllTaskFromDB(Guid SessionUuid)
        {
            var resp = await messenger.SendAsync<TaskResponse>(new TaskMessage() { sessionUuid = SessionUuid }, "task", HttpMethod.Post);
            var tasks = resp.ServerResponse.Tasks;
            return tasks;
        }

        /// <summary>
        /// Zapíše jeden lok. záznam o tasku
        /// </summary>
        /// <param name="task"></param>
        /// <param name="now"></param>
        private void KeepSingle(DbTask task)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(StoreDir, $"Task{task.id}.tsk"), false, Encoding.UTF8))
            {
                writer.Write(task.Serialize());
            }
        }

        /// <summary>
        /// Vytvoří záznamový soubor o času poslední zálohy
        /// </summary>
        /// <param name="time">Čas poslední zálohy</param>
        private void WriteScheme(DateTime time)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(StoreDir, $"Scheme.tsch"), false, Encoding.UTF8))
            {
                writer.Write(time.ToBinary());
                writer.Write("\r\n");
            }
        }

        private DateTime ReadScheme()
        {
            if (!File.Exists(Path.Combine(StoreDir, $"Scheme.tsch")))
                return DateTime.MinValue;
            //try
            //{
            using (StreamReader reader = new StreamReader(Path.Combine(StoreDir, $"Scheme.tsch"), Encoding.UTF8))
            {
                var res = (reader.ReadLine());
                if (res == null)
                    return DateTime.MinValue;
                return DateTime.FromBinary(long.Parse(res));
            }
            /*}
            catch(Exception e)
            {
                logger.Log($"Chyba při načítání lokálních tasků\r\n{e.StackTrace}", LogType.ERROR);
                var log = new GeneralDaemonError() { LogType = LogType.ERROR };
                log.Content.DaemonUuid = new LoginSettings().Uuid;
                log.Content.Message = "Chyba při načítání lokálních záznamů";
                var faf = logger.ServerLogAsync(log);
                File.Delete(Path.Combine(StoreDir, $"Scheme"));
            }*/

        }

        private void Keep(IEnumerable<DbTask> tasks)
        {
            foreach (var task in tasks)
            {
                KeepSingle(task);
            }
        }

        private DbTask ReadTask(string file)
        {
            using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
            {
                var res = (reader.ReadToEnd());
                return DbTask.Deserialize(res);
            }
        }

        private IEnumerable<DbTask> LocalsToLoad(IEnumerable<int> FromServer)
        {
            List<DbTask> tasks = new List<DbTask>();
            foreach (var file in Directory.EnumerateFiles(StoreDir))
            {
                if (Path.GetFileNameWithoutExtension(file).StartsWith("Scheme"))
                    continue;
                bool add = true;
                foreach (var serverFile in FromServer)
                {
                    if (serverFile == int.Parse(Path.GetFileNameWithoutExtension(file).Substring(4)))
                        add = false;
                }
                if (add)
                    tasks.Add(ReadTask(file));
            }
            return tasks;
        }
    
        private void ClearAll()
        {
            foreach (var file in Directory.EnumerateFiles(StoreDir))
            {
                File.Delete(file);
            }
        }

        private async Task<List<DbTask>> Reset(Guid sessionUuid)
        {
            var res = await messenger.SendAsync<TaskResponse>(new TaskMessage() { sessionUuid = sessionUuid,IsDaemon = true}, "Task", HttpMethod.Post);
            ClearAll();
            Keep(res.ServerResponse.Tasks);
            WriteScheme(DateTime.Now);
            return res.ServerResponse.Tasks;
        }

        /// <summary>
        /// Získá vhodné zálohy, jak ze serveru tak lokálně.
        /// Fetchne ze serveru pokud je nutno
        /// </summary>
        /// <param name="sessionUuid">Pro oveření se k serveru</param>
        /// <param name="CTACheck">Kontrola konzistence záznamů</param>
        /// <returns></returns>
        public async Task<List<DbTask>> GetAppropriate(Guid sessionUuid, bool CTACheck = true)
        {
            DateTime time = ReadScheme();
            var changedMsg = new ChangedTaskMessage() { sessionUuid = sessionUuid,LastLoaded = time };
            var res = await messenger.SendAsync<ChangedTaskResponse>(changedMsg, "ChangedTask", HttpMethod.Post);
            WriteScheme(DateTime.Now);
            var localTasks = LocalsToLoad(res.ServerResponse.ChangedTasks.Select(r => r.id));
            Keep(res.ServerResponse.ChangedTasks);
            var result = localTasks.Concat(res.ServerResponse.ChangedTasks).ToList();
            if (result.Count() != res.ServerResponse.CompareTaskAmount && CTACheck)
                return await Reset(sessionUuid);
            return result;
        }
    }
}
