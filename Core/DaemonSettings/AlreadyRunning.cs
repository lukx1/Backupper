using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaemonSettings
{
    public partial class AlreadyRunning : Form
    {
        public AlreadyRunning()
        {
            InitializeComponent();
        }

        public static void OpenDialog()
        {
            var x = new AlreadyRunning();
            x.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
