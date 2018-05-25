using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Daemon.Communication
{
    public class FtpClient : IDisposable
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        private int bufferSize = 4 * 1024;
        public FtpWebResponse ftpResponse { get; set; }

        private Logging.ILogger logger = Logging.LoggerFactory.CreateAppropriate();

        public FtpClient(string host, string username, string password)
        {
            Host = host;
            Username = username;
            Password = Shared.PasswordFactory.DecryptRSA(password, new DaemonShared.LoginSettings().RSAPrivate); 
        }
        /// <summary>
        /// Nahraje soubor přes FTP
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void upload(string source, string destination)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Host + "/" + destination);
                ftpRequest.Credentials = new NetworkCredential(Username, Password);

                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                using (Stream ftpStream = ftpRequest.GetRequestStream())
                {

                    using (FileStream localFileStream = new FileStream(source, FileMode.Create))
                    {
                        byte[] byteBuffer = new byte[bufferSize];
                        int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                        try
                        {
                            while (bytesSent != 0)
                            {
                                ftpStream.Write(byteBuffer, 0, bytesSent);
                                bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                            }
                        }
                        catch (Exception) { logger.Log($"FtpClient failed upload [Source: {source}, Destination {destination}]", Shared.LogType.ERROR); }
                    }
                }
                return;
            }
            finally
            {
                ftpResponse.Close();
            }
        }

        /// <summary>
        /// Vytvoří složku přes FTP
        /// </summary>
        /// <param name="destination"></param>
        public void createDirectory(string destination)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Host + "/" + destination);
                ftpRequest.Credentials = new NetworkCredential(Username, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception)
            {
                logger.Log($"FtpClient failed to create directory [Destination {destination}]", Shared.LogType.ERROR);
            }
            return;
        }

        /// <summary>
        /// Zruší FTP Client
        /// </summary>
        public void Dispose()
        {
            Password = null;
            ftpResponse.Close();
        }
    }
}
