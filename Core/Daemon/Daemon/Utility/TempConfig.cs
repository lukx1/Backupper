using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Utility
{
    public class TempConfig : IConfig
    {
        private string pass = "VO0e+84BW4wqVYsuUpGeWw==";
        private Guid session;
        private Guid uuid = new Guid("50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e");

        public string GetPass()
        {
            return pass;
        }

        public Guid GetSession()
        {
            return session;
        }

        public Guid GetUuid()
        {
            return uuid;
        }

        public void SetPass(string pass)
        {
            this.pass = pass;
        }

        public void SetSession(Guid session)
        {
            this.session = session;
        }

        public void SetUuid(Guid uuid)
        {
            this.uuid = uuid;
        }
    }
}
