using DaemonShared;
using DaemonShared.Pipes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaemonSettings
{
    public partial class Home : Form
    {

        private bool ProfiLoaded = false;
        private Action<PipeCode, object> SendMessage;

        public Home(Action<PipeCode, object> SendMessage)
        {
            InitializeComponent();
            this.SendMessage = SendMessage;
        }

        private void Home_Load(object sender, EventArgs e)
        {

            var settings = new LoginSettings();
            
            try
            {
                settings.Debug = settings.Debug;
            }
            catch(ConfigurationException ex)
            {
                if(ex.InnerException.InnerException is UnauthorizedAccessException)
                {
                    MessageBox.Show(this, "Pro využití tohoto nastavení nemáta dostačující práva\r\nKontaktujte správce Vaší sítě", "Přístup odepřen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Dispose();
                    return;
                }
            }

            /*Introduction stranka*/

            textBoxIIntroductionKod.Text = settings.PreSharedKeyWithIdSemiColSep;
            textBoxIServerAdress.Text = settings.SSLUse ? settings.SSLServer : settings.Server;
            if (!Regex.IsMatch(textBoxIServerAdress.Text, @"^(HTTP:\/\/)|(HTTPS:\/\/)", RegexOptions.IgnoreCase))
                textBoxIServerAdress.Text += settings.SSLUse ? settings.SSLServer : settings.Server;

            /*Hlavni stranka/Daemon stranka*/

            textBoxHHeslo.Text = settings.Password;
            textBoxHJmeno.Text = settings.Uuid.ToString();
            textBoxHServer.Text = settings.Server;
            textBoxHSSLServer.Text = settings.SSLServer;
            checkBoxBackupSSL.Checked = settings.SSLHttpFallback;
            checkBoxSSL.Checked = settings.SSLUse;
        }

        private void buttonIUlozit_Click(object sender, EventArgs e)
        {
            var introductionCode = Regex.Replace(textBoxIIntroductionKod.Text, @"\s+", "");
            var address = Regex.Replace(textBoxIServerAdress.Text, @"\s+", "");
            if (address.ToUpper().StartsWith("HTTPS"))
            {
                var settings = new LoginSettings();
                settings.PreSharedKeyWithIdSemiColSep = introductionCode;
                settings.SSLUse = true;
                settings.SSLServer = address;
            }
            else if (address.ToUpper().StartsWith("HTTP"))
            {
                var settings = new LoginSettings();
                settings.PreSharedKeyWithIdSemiColSep = introductionCode;
                settings.SSLUse = true;
                settings.SSLServer = address;
            }

        }

        private void buttonHUlozit_Click(object sender, EventArgs e)
        {
            var settings = new LoginSettings();

            settings.Password = textBoxHHeslo.Text;
            settings.Uuid = new Guid(textBoxHJmeno.Text);
            settings.Server = textBoxHServer.Text;
            settings.SSLServer = textBoxHSSLServer.Text;
            settings.SSLHttpFallback = checkBoxBackupSSL.Checked;
            settings.SSLUse = checkBoxSSL.Checked;
        }

        private bool IsValid(LoginSettings settings, string name, object val)
        {
            return true;
        }

        private void LoadProSettings()
        {
            var settings = new LoginSettings();
            IEnumerator enumerator = settings.Properties.GetEnumerator();


            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Jméno", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Hodnota", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            while (enumerator.MoveNext())
            {
                SettingsProperty e = (SettingsProperty)enumerator.Current;
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView);
                row.Cells[0].Value = e.Name;
                row.Cells[1].Value = settings[e.Name] == null ? "" : settings[e.Name].ToString();
                dataGridView.Rows.Add(row);
                //listBoxValues.Items.Add();
            }
        }

        private bool PushChanges()
        {
            var settings = new LoginSettings();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var name = row.Cells[0].Value.ToString();
                var value = row.Cells[1].Value;
                if (!IsValid(settings, name, value))
                    return false;
            }
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var name = row.Cells[0].Value.ToString();
                var value = row.Cells[1].Value;
                try
                {
                    if (settings.Properties[name].PropertyType == typeof(bool))
                        settings[name] = bool.Parse(value.ToString());
                    else if (settings.Properties[name].PropertyType == typeof(Guid))
                        settings[name] = Guid.Parse(value.ToString());
                    else if (settings.Properties[name].PropertyType == typeof(int))
                        settings[name] = int.Parse(value.ToString());
                    else if (settings.Properties[name].PropertyType == typeof(string))
                        settings[name] = value.ToString();
                    else if (settings.Properties[name].PropertyType == typeof(DateTime))
                        settings[name] = DateTime.Parse(value.ToString());
                }
                catch(FormatException e)
                {
                    MessageBox.Show(null, $"Neplatná hodnota v poli {name}", "Chyba v ukládání", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            settings.Save();
            return true;
        }

        private void buttonPUlozit_Click(object sender, EventArgs e)
        {
            if (PushChanges())
            {
                MessageBox.Show(this, $"Uloženo", "Úspešně uloženo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl.SelectedIndex == 2 && !ProfiLoaded)
            {
                ProfiLoaded = true;
                LoadProSettings();
            }
        }

        public void MessageReceived(PipeCode p, string s)
        {
            if(p == PipeCode.LOGIN_RESPONSE)
            {
                var r = PipeLoginResponse.Deserialize(s); ;
                if (r.B)
                    MessageBox.Show(this, $"Klíč úspěšně nastaven", "Úspešně nastaveno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, $"Nebylo možno se přihlásit", "Nastavení selhalo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonKOK_Click(object sender, EventArgs e)
        {
            SendMessage(PipeCode.USER_LOGIN, new PipeLoginAttempt() { U = textBoxKUsername.Text, P = textBoxKPassword.Text });
        }
    }
}
