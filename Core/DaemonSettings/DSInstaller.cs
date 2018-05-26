using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DaemonSettings
{
    [RunInstaller(true)]
    public partial class DSInstaller : System.Configuration.Install.Installer
    {
        public DSInstaller()
        {
            InitializeComponent();
        }

        private void DSInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue("BackupperDS", Context.Parameters["assemblyPath"]);
            //Process.Start(Context.Parameters["assemblyPath"]);
        }

        private void DSInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.DeleteValue("BackupperDS", false);
        }

        private void DSInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            Process[] pnames = Process.GetProcessesByName("DaemonSettings.exe");
            foreach (var proc in pnames)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception) { }
            }
            Process[] psnames = Process.GetProcessesByName("DaemonSettings");
            foreach (var proc in psnames)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception) { }
            }
            Process[] pssnames = Process.GetProcessesByName("DaemonSettings (32 bit)");
            foreach (var proc in pssnames)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception) { }
            }
            Process.Start(Context.Parameters["assemblyPath"]);
        }
    }
}
