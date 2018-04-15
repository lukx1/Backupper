using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaemonSettings
{
    public class NotifyCreator
    {
        private NotifyIcon notifyIcon;
        private Action settingsA;
        private Action exitA;

        public static void ShowAlreadyRunningDialog()
        {
            AlreadyRunning r = new AlreadyRunning();
            r.ShowDialog();
        }

        public void Create(Action settingsA, Action exitA)
        {
            this.settingsA = settingsA;
            this.exitA = exitA;

            if (notifyIcon == null)
            {
                notifyIcon = new NotifyIcon();
                notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
                notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                notifyIcon.BalloonTipText = "Nastavení zálohování Backupperu";
                notifyIcon.BalloonTipTitle = "Backupper";
                notifyIcon.Icon = Properties.Resources.MainIcon;

                var contextMenu = new ContextMenu();

                var mItem0 = new MenuItem() { Index = 0, Text = "Nastavení" };
                mItem0.Click += (o,e) => settingsA();
                var mItem1 = new MenuItem() { Index = 1, Text = "E&xit" };
                mItem1.Click += (o, e) => exitA();

                contextMenu.MenuItems.AddRange(new MenuItem[] { mItem0, mItem1 });

                notifyIcon.ContextMenu = contextMenu;
                notifyIcon.Visible = true;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            settingsA();
        }
    }
}
