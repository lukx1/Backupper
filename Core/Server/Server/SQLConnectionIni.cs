using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    public class SQLConnectionIni
    {
        private IniManipulator ini = new IniManipulator(Shared.Util.GetFileInAppData("SQLConnect.ini"));

        public string Server { get => ini.Read("Server"); set => ini.Write("Server", value); }
        public string Database { get => ini.Read("Database"); set => ini.Write("Database", value); }
        public string UserID { get => ini.Read("UserID"); set => ini.Write("UserID", value); }
        public string Password { get => ini.Read("Password"); set => ini.Write("Password", value); }
    }
}