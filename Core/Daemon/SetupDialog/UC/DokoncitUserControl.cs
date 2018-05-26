using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DaemonShared;
using System.IO;
using Shared;

namespace SetupDialog
{
    public partial class DokoncitUserControl : UserControl
    {
        private HowToSetup howToSetup;

        public DokoncitUserControl(HowToSetup howToSetup)
        {
            this.howToSetup = howToSetup;
            InitializeComponent();
        }

        public delegate void DokoncitClickedDelagte();
        public event DokoncitClickedDelagte DokoncitClicked;

        private void button1_Click(object sender, EventArgs e)
        {
            var dir = Path.Combine(Util.GetSharedFolder(), "Install");
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, "transfer.tsf");
            File.WriteAllText(
                file,
                $"{howToSetup.Password};" +
                $"{howToSetup.Username};" +
                $"{howToSetup.PrivateKey};" +
                $"{howToSetup.Server}"
                );
            DokoncitClicked();
        }
    }
}
