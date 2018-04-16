using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonSettings
{
    public class NotifyWaiter
    {
        [STAThread]
        public void Run(Action openSettings,Action exitA)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            NotifyCreator nc = new NotifyCreator();
            nc.Create(openSettings,exitA);
            System.Windows.Forms.Application.Run();
        }
    }
}
