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
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "IIS Express", "iisexpress.exe");
            }
            else // 32 bit
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "IIS Express", "iisexpress.exe");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Loading settings");
            var settings = new Shared.Properties.SharedSettings();
            Console.WriteLine("Settings loaded");
            var iiseInstallerPath = Path.GetTempFileName()+".msi";
            var serverInstallerPath = Path.GetTempFileName()+".msi";
            Console.WriteLine("Got iiseIP name  :"+iiseInstallerPath);
            Console.WriteLine("Got serverIP name:"+serverInstallerPath);
            if (!File.Exists(getIISExpressFile()))
            {
                Console.WriteLine("IISE doesnt exist, starting install");
                File.WriteAllBytes(iiseInstallerPath, Environment.Is64BitOperatingSystem ? Resource.iisexpress_amd64_en_US : Resource.iisexpress_x86_en_US);
                {
                    Console.WriteLine("Installing IISE");
                    var proc = new Process();
                    proc.StartInfo.FileName = iiseInstallerPath;
                    proc.Start();
                    proc.WaitForExit();
                    Console.WriteLine("IISE finished");
                }
                settings.SIISexExe = getIISExpressFile();
                File.Delete(iiseInstallerPath);
                Console.WriteLine("IISE Installer removed");
            }
            settings.Save();
            Console.WriteLine("Started server install");
            File.WriteAllBytes(serverInstallerPath, Resource.Server_IISEx_Setup2);
            {
                Console.WriteLine("Installing server");
                var proc = new Process();
                proc.StartInfo.FileName = serverInstallerPath;
                proc.Start();
                proc.WaitForExit();
                Console.WriteLine("Server finished");
            }
            File.Delete(serverInstallerPath);
            Console.WriteLine("Server Installer removed");
            Console.WriteLine("Starting IISE");
            Process.Start(getIISExpressFile(), $"/path:\"{settings.SInstallDirPath}\" /port:{settings.SHttpPort}");
        }
    }
}
