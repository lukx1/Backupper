using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared.NetMessages;
using System.Net.Http;
using Shared;

namespace SetupDialog
{
    public partial class ServerUserControl : UserControl
    {
        private HowToSetup how;

        public ServerUserControl(HowToSetup how)
        {
            this.how = how;
            InitializeComponent();
        }

        public delegate void DalsiClickedDel();
        public event DalsiClickedDel DalsiClicked;

        private async Task<bool> TestServer(string server)
        {
            var messenger = new Messenger(server);
            try
            {
                var res = await messenger.SendAsync<UserLoginResponse>(new UserLoginMessage() { Username = "1234", Password = "1234" }, "UserLogin", HttpMethod.Post);
            }
            catch(INetException<UserLoginResponse> ex)
            {

            }
            catch(Exception e)
            {
                MessageBox.Show(this, "Nebylo možné server kontaktovat", "Připojení selhalo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tsk = Task.Run( async () =>
            {
                var server = this.textBoxServer.Text;
                var resp = await TestServer(server);
                if (!resp)
                {
                    return false;
                }
                how.Server = server;
                return true;
            });
            tsk.Wait();
            if (tsk.Result)
            {
                DalsiClicked();
                Dispose();
            }
        }
    }
}
