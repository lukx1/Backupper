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
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "iisexpress.exe");
            }
            else // 32 bit
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "iisexpress.exe");
            }
        }

        private void Installer_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                string dir = Directory.GetParent(Context.Parameters["assemblyPath"]).FullName;
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
                    var proc = Process.Start(installer, "/quiet /promptrestart");
                    proc.WaitForExit(10000);
                }

                Task.Run(() =>
                {
                    if (Environment.Is64BitOperatingSystem)
                    {
                        Process.Start(Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "iisexpress.exe"), $"/path:\"{dir}\" /port:3393");
                    }
                    else // 32 bit
                {
                        Process.Start(Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "iisexpress.exe"), $"/path:\"{dir}\" /port:3393");
                    }
                });
            }
            catch(Exception ex)
            {
                File.WriteAllText(@"C:\AAA\crash.log", ex.ToString());
            }
        }

        private void Installer_AfterUninstall(object sender, InstallEventArgs e)
        {
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
            catch (Exception) { }
        }
    }
}
