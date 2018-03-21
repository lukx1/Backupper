﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Daemon.Backups
{
    public class BackupInfo
    {
        public DateTime timeCreated { get; set; }
        public IBackup pBackup { get; set; }
        public List<FileBackupedInfo> files { get; set; }
        public string rootFolderPath { get; set; }
        public string rootBackupPath { get; set; }

        public BackupInfo(IBackup backup)
        {
            pBackup = backup;
            files = new List<FileBackupedInfo>();
        }

        public void CreateFile(string destination)
        {
            timeCreated = DateTime.Now;
            CreateBackupInfo("");
            StreamWriter writer = new StreamWriter(destination + ".log");
            writer.WriteLine(timeCreated.ToString());
            writer.WriteLine(pBackup.ID);
            writer.WriteLine(pBackup.SourcePath);
            foreach (FileBackupedInfo item in files)
            {
                writer.WriteLine($"{item.subRootPath};{item.name};{item.size}");
            }
            writer.Close();
        }

        public void CreateBackupInfo(string sub)
        {
            string dir = pBackup.SourcePath;
            string subDir = sub;

            foreach (FileInfo item in new DirectoryInfo(dir + subDir).GetFiles())
            {
                files.Add(new FileBackupedInfo() { subRootPath = subDir, name = item.Name, size = (int)item.Length });
            }
            foreach (DirectoryInfo item in new DirectoryInfo(dir + subDir).GetDirectories())
            {
                CreateBackupInfo(sub + "/" + item.Name);
            }
        }

    }
}
