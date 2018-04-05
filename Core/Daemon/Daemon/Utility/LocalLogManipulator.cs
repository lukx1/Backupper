using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Utility
{
    public class LocalLogManipulator
    {
        private readonly string StoreFolder = Path.Combine(Util.GetAppdataFolder(),"LocalLogs");

        private static string GetOrCreateStoreFolder()
        {
            var path = Path.Combine(Util.GetAppdataFolder(), "LocalLogs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public void Store<T>(ILog<T> log) where T: class
        {
            var json = JsonableUniversalLog.CreateFrom(log);
            var destFolder = Path.Combine(StoreFolder,log.Code.Uuid.ToString());
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            var dest = Path.Combine(destFolder, $"{DateTime.Now.ToString().Replace(':', '.')}-crash.log");
            File.WriteAllText(dest, json.ToJson(),Encoding.UTF8);
        }

        public IEnumerable<JsonableUniversalLog> ReadAllLogs(bool deleteAfterRead = true)
        {
            foreach (var dir in Directory.EnumerateDirectories(StoreFolder))
            {
                foreach (var file in Directory.EnumerateFiles(dir))
                {
                    var json = File.ReadAllText(file, Encoding.UTF8);
                    var res = JsonableUniversalLog.FromJson(json);
                    yield return res;
                    if (deleteAfterRead)
                        File.Delete(file);
                }
            }
        }

        public IEnumerable<JsonableUniversalLog> ReadLogs(LogContentType logType, bool deleteAfterRead = true)
        {
            var readDir = Path.Combine(StoreFolder, logType.Uuid.ToString());
            if (!Directory.Exists(readDir))
                yield break;
            foreach (var file in Directory.EnumerateFiles(readDir))
            {
                var json = File.ReadAllText(file);
                var res = JsonableUniversalLog.FromJson(json);
                yield return res;
                if (deleteAfterRead)
                    File.Delete(file);
            }
        }

    }
}
