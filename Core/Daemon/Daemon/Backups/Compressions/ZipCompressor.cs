using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.NetMessages.TaskMessages;
using System.IO;
using System.IO.Compression;

namespace Daemon.Backups.Compressions
{
    class ZipCompressor : ICompression
    {
        public DbTaskDetails taskDetails { get; set; }
        public SmartBackupInfo backupInfo { get; set; }

        public ZipCompressor(DbTaskDetails taskDetails, SmartBackupInfo smartBackupInfo)
        {
            this.taskDetails = taskDetails;
            this.backupInfo = smartBackupInfo;
        }

        public string Compress()
        {
            foreach (SmartFileInfo item in backupInfo.fileInfos)
            {
                string name = DateTime.Now.ToFileTimeUtc().ToString();
                Directory.CreateDirectory(Shared.Util.GetAppdataFolder() + "\\" + name);

                FileInfo fileToCompress = new FileInfo(item.destination);

                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                        FileInfo info = new FileInfo(Shared.Util.GetAppdataFolder() + "\\" + name + "\\" + fileToCompress.Name + ".gz");
                    }

                }
            }



            throw new NotImplementedException();
        }
    }
}