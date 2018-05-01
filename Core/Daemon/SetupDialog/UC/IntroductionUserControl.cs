using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetupDialog
{
    public partial class IntroductionUserControl : UserControl
    {
        private HowToSetup howToSetup;

        public IntroductionUserControl(HowToSetup howToSetup)
        {
            this.howToSetup = howToSetup;
            InitializeComponent();
        }

        public delegate void DalsiClickedDelagate();
        public event DalsiClickedDelagate DalsiClicked;


        private void buttonDalsi_Click(object sender, EventArgs e)
        {
            howToSetup.PreSharedKey = this.textBox1.Text;    
            DalsiClicked();
        }
    }
}
