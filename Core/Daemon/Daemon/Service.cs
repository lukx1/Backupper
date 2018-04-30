using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public partial class Service : ServiceBase
    {
        public Service()
        { 
            InitializeComponent();
            
        }

        public void OnPubStart(string[] args = null) => OnStart(args);
        public void OnPubStop() => OnStop();

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            Task.Run(() => { Core core = new Core(); });
        }

        protected override void OnStop()
        {
        }
    }
}
