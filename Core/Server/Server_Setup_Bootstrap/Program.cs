using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Setup_Bootstrap
{
    class Program
    {
        static private string getIISExpressFile()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "iisexpress.exe");
            }
            else // 32 bit
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "iisexpress.exe");
            }
        }

        static void Main(string[] args)
        {
            var settings = new Shared.Properties.SharedSettings();
            if (!File.Exists(getIISExpressFile()))
            {
                File.WriteAllBytes("iise.msi", Environment.Is64BitOperatingSystem ? Resource.iisexpress_amd64_en_US : Resource.iisexpress_x86_en_US);
                {
                    var proc = new Process();
                    proc.StartInfo.FileName = "iise.msi";
                    proc.Start();
                    proc.WaitForExit();
                }
                settings.SIISexExe = getIISExpressFile();
                File.Delete("iise.msi");
            }
            File.WriteAllBytes("server.msi", Resource.Server_IISEx_Setup2);
            {
                var proc = new Process();
                proc.StartInfo.FileName = "server.msi";
                proc.Start();
                proc.WaitForExit();
            }
            File.Delete("server.msi");
            Process.Start(getIISExpressFile(), $"/path:\"{settings.SInstallDirPath}\" /port:{settings.SHttpPort}");
        }
    }
}
