using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetupDialog
{
    public partial class Setup : Form
    {
        private int Stage = 0;
        private HowToSetup howToSetup = new HowToSetup();

        public Setup()
        {
            InitializeComponent();
            ServerUserControl sc = new ServerUserControl(howToSetup);
            sc.DalsiClicked += Sc_DalsiClicked;
            sc.Dock = DockStyle.Fill;
            this.Controls.Add(sc);
        }

        private void Next()
        {
            this.Controls.Clear();
            switch (Stage){
                case 0:  
                    UserUserControl userControl = new UserUserControl(howToSetup);
                    userControl.DalsiClicked += Sc_DalsiClicked;
                    userControl.Dock = DockStyle.Fill;
                    this.Controls.Add(userControl);
                    break;
                case 1:
                    if (howToSetup.UseOCI)
                    {
                        Stage++;
                        Next();
                        return;
                    }
                    else
                    {
                        IntroductionUserControl uc = new IntroductionUserControl(howToSetup);
                        uc.DalsiClicked += Sc_DalsiClicked;
                        uc.Dock = DockStyle.Fill;
                        this.Controls.Add(uc);
                    }
                    break;
                case 2:
                    DokoncitUserControl dokoncitUserControl = new DokoncitUserControl(howToSetup);
                    dokoncitUserControl.DokoncitClicked += DokoncitUserControl_DokoncitClicked;
                    dokoncitUserControl.Dock = DockStyle.Fill;
                    this.Controls.Add(dokoncitUserControl);
                    break;

            }
            Stage++;
        }

        private void DokoncitUserControl_DokoncitClicked()
        {
            ServiceController service = new ServiceController("Backupper");
            service.Start();
            Dispose();
        }

        private void Sc_DalsiClicked()
        {
            Next();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Setup_Load(object sender, EventArgs e)
        {

        }
    }
}
