using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shared.NetMessages.TaskMessages;
using Shared;

namespace Daemon.Backups
{
    public class SmartBackupInfo
    {
        public static string StorePath = Path.Combine(Util.GetAppdataFolder(), "Data", "BackupInfos");

        public List<SmartFileInfo> fileInfos { get; set; }
        public DbTaskLocation location { get; set; }

        public SmartBackupInfo()
        {
            Directory.CreateDirectory(StorePath);// Pojištění existence
            fileInfos = new List<SmartFileInfo>();
        }

        /// <summary>
        /// Vytvoří BackupInfo VŠECH souborů v definované cestě
        /// </summary>
        /// <param name="path"></param>
        public void CreateFullBackupInfo(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo item in dir.GetFiles())
            {
                fileInfos.Add(new SmartFileInfo()
                {
                    filename = item.Name,
                    destination = item.FullName,
                    lastDateModified = item.LastWriteTime
                });
            }
            foreach (DirectoryInfo item in dir.GetDirectories())
            {
                CreateFullBackupInfo(item.FullName);
            }
        }

        /// <summary>
        /// Porovná BackupInfa a vybere jen soubory co jsou v tomto BackupInfu a nejsou/odlišují se od daného v parametru
        /// </summary>
        /// <param name="info"></param>
        public void Differentiate(SmartBackupInfo info)
        {
            List<SmartFileInfo> temp = new List<SmartFileInfo>();

            foreach (SmartFileInfo item in fileInfos)
            {
                bool exists = false;
                foreach (SmartFileInfo file in info.fileInfos)
                {
                    if (item.destination == file.destination && item.lastDateModified == file.lastDateModified)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    temp.Add(item);
            }
            fileInfos = temp;
        }


        public string GetOldestBackupPath()
        {
            string path = "";

            DirectoryInfo dir = new DirectoryInfo(StorePath);
            FileInfo[] files = dir.GetFiles();

            DateTime time = DateTime.MaxValue;
            foreach (FileInfo item in files)
            {
                string[] name = item.Name.Split('_');
                if (Convert.ToInt32(name[0]) == location.id && (item.LastWriteTime < time))
                {
                    time = item.LastWriteTime;
                    path = item.FullName;
                }
            }

            return path;
        }

        public void WriteToFile(string path)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(location.id);
                writer.WriteLine(location.source.uri);
                writer.WriteLine(location.destination.uri);
                foreach (SmartFileInfo item in fileInfos)
                    writer.WriteLine($"{item.destination};{item.lastDateModified}");
            }
        }

        public void ReadFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                if (reader.ReadLine() != location.id.ToString())
                    throw new Exception("id doesnt match");
                if (reader.ReadLine() != location.source.uri)
                    throw new Exception("source doesnt match");
                if (reader.ReadLine() != location.destination.uri)
                    throw new Exception("destination doesnt match");

                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(';');
                    FileInfo info = new FileInfo(data[0]);
                    fileInfos.Add(new SmartFileInfo() { destination = info.FullName, filename = info.Name, lastDateModified = info.LastWriteTime });
                }
            }
        }

        public void ReadOldestSimilar()
        {
            string temp = GetOldestBackupPath();
            if (temp != "")
                ReadFromFile(temp);
        }

        public void UnionAllSimilarInfos()
        {
            List<SmartBackupInfo> infos = new List<SmartBackupInfo>();

            DirectoryInfo dir = new DirectoryInfo(StorePath);
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo item in files)
            {
                string[] name = item.Name.Split('_');
                if (Convert.ToInt32(name[0]) == location.id)
                {
                    SmartBackupInfo temp = new SmartBackupInfo() { location = this.location };
                    temp.ReadFromFile(item.FullName);
                    infos.Add(temp);
                }
            }
            
            foreach (SmartBackupInfo item in infos)
                this.Differentiate(item);
        }

    }
}
