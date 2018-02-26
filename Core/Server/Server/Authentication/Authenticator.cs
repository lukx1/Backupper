using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    public class Authenticator
    {

        private MySQLContext mysql;

        public Authenticator()
        {
            this.mysql = new MySQLContext();
        }

        public Daemon GetDaemonFromUuid(Guid uuid)
        {
            LogedInDaemon logedInDaemon = mysql.LogedInDaemons.Where(r => r.SessionUuid == uuid).FirstOrDefault();
            return mysql.Daemons.Where(r => r.Id == logedInDaemon.IdDaemon).First();
        }

        public bool IsSessionValid(Guid uuid)
        {
            LogedInDaemon logedInDaemon = mysql.LogedInDaemons.Where(r => r.SessionUuid == uuid).FirstOrDefault();
            if (logedInDaemon == null)
                return false;
            if (Util.IsExpired(logedInDaemon.Expires))
                return false;
            return true;
        }

    }
}