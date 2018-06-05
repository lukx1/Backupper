using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.NetMessages.TaskMessages;

namespace Daemon.Backups.Compressions
{
    public class Compressor
    {
        public SmartBackupInfo backupInfo { get; set; }
        public DbTaskDetails taskDetails { get; set; }

        public Compressor(SmartBackupInfo backupInfo, DbTaskDetails taskDetails)
        {
            this.backupInfo = backupInfo;
            this.taskDetails = taskDetails;
        }

        public SmartBackupInfo Compress(string destination,CompressionLevel compressionLevel)
        {
            int trim = backupInfo.location.source.uri.Length;

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in backupInfo.fileInfos)
                    {
                        string fileName = file.destination.Substring(trim, file.destination.Length - trim); 

                        var demoFile = archive.CreateEntry(fileName,compressionLevel);

                        using (var entryStream = demoFile.Open())
                        using (var b = new BinaryWriter(entryStream))
                        {
                            b.Write(File.ReadAllBytes(file.destination));
                        }
                    }
                }

                using (var fileStream = new FileStream(destination, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }

                SmartBackupInfo temp = new SmartBackupInfo() { location = new DbTaskLocation() { source = new DbLocation() { uri = Path.GetTempPath() } } };
                temp.fileInfos.Add(new SmartFileInfo() { destination = destination, filename = Path.GetFileName(destination) });

                return temp;
                
            }
        }

    }
}
