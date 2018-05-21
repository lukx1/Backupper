using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Utility
{
    /// <summary>
    /// Vytváří lokální logy o chybách
    /// </summary>
    public class LocalLogManipulator
    {
        private readonly string StoreFolder = Path.Combine(Util.GetAppdataFolder(),"LocalLogs");

        /// <summary>
        /// Kontroluje jestli existuje LocalLogs v Daemonovi v Appdata
        /// pokud ano, je vytvořen
        /// </summary>
        public LocalLogManipulator()
        {
            Directory.CreateDirectory(StoreFolder);
        }

        private static string GetOrCreateStoreFolder()
        {
            var path = Path.Combine(Util.GetAppdataFolder(), "LocalLogs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Uloží log lokálně
        /// </summary>
        /// <typeparam name="T">Druh logů</typeparam>
        /// <param name="log">Log</param>
        public void Store<T>(SLog<T> log) where T: class
        {
            var json = JsonableUniversalLog.CreateFrom(log);
            var destFolder = Path.Combine(StoreFolder,log.Code.Uuid.ToString());
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            var dest = Path.Combine(destFolder, $"{DateTime.Now.ToString().Replace(':', '.')}-crash.log");
            File.WriteAllText(dest, json.ToJson(),Encoding.UTF8);
        }

        /// <summary>
        /// Přečte všechny lokálně uložené logy
        /// </summary>
        /// <param name="deleteAfterRead">Pokud vše ctěné má být smazáno</param>
        /// <returns>Lokální logy nebo prázndou kolekci, když nejsou</returns>
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

        /// <summary>
        /// Přečte specifické logy
        /// </summary>
        /// <param name="logType">Druh logu</param>
        /// <param name="deleteAfterRead">Smazat po přečtení</param>
        /// <returns>Lokální logy nebo prázndou kolekci, když nejsou</returns>
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
