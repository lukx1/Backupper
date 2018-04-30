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
            Process.Start(Context.Parameters["assemblyPath"]);
        }

        private void DSInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.DeleteValue("BackupperDS", false);
        }
    }
}
