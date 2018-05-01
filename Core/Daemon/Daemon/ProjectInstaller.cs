using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Daemon
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            try
            {
                if (EventLog.SourceExists("Backupper"))
                {
                    if (EventLog.Exists("Backupper"))
                        EventLog.Delete("Backupper");
                    EventLog.DeleteEventSource("Backupper");
                }
            }
            catch (Exception e) { }
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void serviceInstaller1_BeforeUninstall(object sender, InstallEventArgs e)
        {
            try
            {
                System.Diagnostics.EventLog.Delete("Backupper");
            }
            catch (Exception) { }
            try
            {
                System.Diagnostics.EventLog.DeleteEventSource("Backupper");
            }
            catch (Exception) { }
        }
    }
}
