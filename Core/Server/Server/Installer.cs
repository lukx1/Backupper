using Shared.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        private string getIISExpressFile()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"),"IIS Express", "iisexpress.exe");
            }
            else // 32 bit
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"),"IIS Express", "iisexpress.exe");
            }
        }

        private void Installer_AfterInstall(object sender, InstallEventArgs e)
        {
            return;
            var settings = new Shared.Properties.SharedSettings();
            settings.SInstallDirPath = Directory.GetParent(Context.Parameters["assemblyPath"]).Parent.FullName;
            settings.Save();
            /*string dir = Path.GetDirectoryName(Context.Parameters["assemblyPath"]);
            var logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DaemonSetup", "Install.log");
            var settings = new SharedSettings();
            Task.Run(() =>
            {

                if (Environment.Is64BitOperatingSystem)
                {
                    var rPath = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "IIS Express", "iisexpress.exe");
                    settings.SIISexExe = rPath;
                    File.AppendAllText(logFile, $"Started {rPath} /path:\"{ dir}\" /port:3393\r\n");
                    Process.Start(rPath, $"/path:\"{dir}\" /port:3393");

                }
                else // 32 bit
                    {
                    var rPath = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "IIS Express", "iisexpress.exe");
                    settings.SIISexExe = rPath;
                    File.AppendAllText(logFile, $"Started {rPath} /path:\"{ dir}\" /port:3393\r\n");
                    Process.Start(rPath, $"/path:\"{dir}\" /port:3393");
                }
            });

            */
        }

        private void Installer_AfterUninstall(object sender, InstallEventArgs e)
        {
            return;
            /*var settings = new SharedSettings();
            try
            {
                string dir = Path.GetDirectoryName(Context.Parameters["assemblyPath"]);
                string file = "iisexpress_x86_en-US.msi";
                if (Environment.Is64BitOperatingSystem)
                {
                    file = "iisexpress_amd64_en-US.msi";
                }
                else // 32 bit
                {
                    file = "iisexpress_x86_en-US.msi";
                }
                Process.Start(Path.Combine(dir, "iisex", file), "/quiet /promptrestart /x Product.msi");
            }
            catch (Exception) { }*/
        }

        private void Installer_BeforeInstall(object sender, InstallEventArgs e)
        {
            return;
            /*var logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DaemonSetup", "Install.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logFile));
            File.AppendAllText(logFile, "IISEX:" + getIISExpressFile() + "\r\n");
            var settings = new SharedSettings();
            string dir = Directory.GetParent(Context.Parameters["assemblyPath"]).Parent.FullName;
            settings.SInstallDirPath = dir;
            string file = "iisexpress_x86_en-US.msi";
            if (Environment.Is64BitOperatingSystem)
            {
                file = "iisexpress_amd64_en-US.msi";
            }
            else // 32 bit
            {

                file = "iisexpress_x86_en-US.msi";
            }
            if (!File.Exists(getIISExpressFile()))
            {
                var installer = Path.Combine(dir, "iisex", file);
                File.AppendAllText(logFile, "Starting:" + installer);
                var proc = Process.Start(installer);
                File.AppendAllText(logFile, "Waiting for install\r\n");
                proc.WaitForExit(-1);
                File.AppendAllText(logFile, "Finished waiting\r\n");
            }
            settings.Save();*/
        }
    }
}
