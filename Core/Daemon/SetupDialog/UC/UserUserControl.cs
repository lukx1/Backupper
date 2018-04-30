using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using Shared.NetMessages;
using System.Net.Http;

namespace SetupDialog
{
    public partial class UserUserControl : UserControl
    {
        private HowToSetup how;

        public UserUserControl(HowToSetup howToSetup)
        {
            this.how = howToSetup;
            InitializeComponent();
        }

        public delegate void DalsiClickedDelagte();
        public event DalsiClickedDelagte DalsiClicked;

        private string CheckLogin(string name, string pass)
        {
            var messenger = new Messenger(how.Server);
            var task = messenger.SendAsync<UserLoginResponse>(new UserLoginMessage() {Username = name,Password = pass },"UserLogin",HttpMethod.Post);
            task.Wait();
            var res = task.Result.ServerResponse;
            if (!res.OK)
            {
                MessageBox.Show(null, "Neplatné přihlašovací údaje", "Přihlášení", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return res.PrivateKeyEncrypted;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var task = Task.Run(() =>
            {
                var username = this.textBoxJmeno.Text;
                var heslo = this.textBoxHeslo.Text;
                string pke;
                if ((pke = CheckLogin(username, heslo)) == null)
                    return false;
                how.PrivateKey = PasswordFactory.DecryptAES(pke, heslo);
                how.Password = heslo;
                how.Username = username;
                how.UseOCI = this.checkBoxOCI.Checked;
                return true;
            });
            task.Wait();
            if (!task.Result)
                return;
            DalsiClicked();
        }
    }
}
