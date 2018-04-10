using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;

namespace Daemon.Communication
{
    public class SftpClient
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port = 22;

        private Logging.ILogger logger = Logging.LoggerFactory.CreateAppropriate();

        public SftpClient(string host, string username, string password)
        {
            Host = host;
            Username = username;
            Password = password;
        }

        public void Upload(string sourceFile, string destinationPath)
        {
            using (Renci.SshNet.SftpClient client = new Renci.SshNet.SftpClient(Host,Username,Password))
            {
                try
                {
                    client.Connect();
                }
                catch(Exception)
                {
                    logger.Log($"SftpClient failed to connect [Host: {Host},User: {Username},Password: {Password}] while Uploading File",Shared.LogType.ERROR);
                }

                try
                {
                    client.ChangeDirectory(destinationPath);
                    using (FileStream stream = new FileStream(sourceFile, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(stream, Path.GetFileName(sourceFile));
                    }
                }
                catch (Exception)
                {
                    logger.Log($"SftpClient failed to upload file [DestinationPath: {destinationPath},SourceFile: {sourceFile}]", Shared.LogType.ERROR);
                }
            }
        }

        public void CreateDirectory(string destination)
        {
            using (Renci.SshNet.SftpClient client = new Renci.SshNet.SftpClient(Host, Username, Password))
            {
                try
                {
                    client.Connect();
                }
                catch (Exception)
                {
                    logger.Log($"SftpClient failed to connect [Host: {Host},User: {Username},Password: {Password}] while CreatingDirectory", Shared.LogType.ERROR);
                }

                try
                {
                    client.CreateDirectory(destination);
                }
                catch (Exception)
                {
                    logger.Log($"SftpClient failed to create directory [Destination: {destination}]", Shared.LogType.ERROR);
                }
            }
        }
    }
}
