using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.Objects;

namespace Server.Models.Admin
{
    public class PublicInfo
    {
        private bool _hasOCI;
        private UptimeCalculator.Uptime _uptime;
        private string _serverName;
        private string _userName;

        public UptimeCalculator.Uptime Uptime => _uptime;
        public bool HasOCI => _hasOCI;
        public string ServerName => _serverName;
        public string UserName => _userName;

        public void Load()
        {
            _hasOCI = true;
            _uptime = new UptimeCalculator().Calculate();
            _serverName = Environment.MachineName;
            _userName = Environment.UserName;
        }
    }
}