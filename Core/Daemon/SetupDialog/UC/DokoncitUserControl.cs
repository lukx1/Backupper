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
            LoginSettings settings = new LoginSettings();
            settings.Password = howToSetup.Password;
            settings.OwnerUserNickname = howToSetup.Username;
            settings.RSAPrivate = howToSetup.PrivateKey;
            settings.Server = howToSetup.Server;
            DokoncitClicked();
        }
    }
}
