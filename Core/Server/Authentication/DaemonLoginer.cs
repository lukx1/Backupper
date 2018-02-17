using Server.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    public class DaemonLoginer
    {
        private MySQLContext mysql;
        /// <summary>
        /// Login extension period in minutes
        /// </summary>
        public const long LOGIN_PERIOD = 15;

        public DaemonLoginer()
        {
            mysql = MySQLContext.Instance;
        }

        private void Validate(Daemon daemon, string password)
        {
            if (daemon == null)
                throw new ArgumentNullException("Daemons s daným uuid neexistuje");
            if (!IsPasswordValid(password, daemon))
                throw new ArgumentException("Hesla se neshodují");
        }

        /// <summary>
        /// Prihlasi daemona, hazi ArgumentException nebo ArgumentNullexception pokud nastane chyba
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="password"></param>
        public void Login(Guid uuid, string password)
        {
            Daemon daemon = mysql.daemons.Where(r => r.uuid == uuid).FirstOrDefault();
            LogedInDaemon logedInDaemon = GetLogedInDaemonWithUuid(uuid);

            Validate(daemon,password);

            if (logedInDaemon == null) // Daemon existuje ale nikdy nebyl prihlasen
                FirstLogin(daemon);
            else // Daemon existuje a bude prihlasen na loginPrediod
                logedInDaemon.expires = DateTime.Now.AddMinutes(LOGIN_PERIOD);
            mysql.SaveChanges();
        }

        private bool IsPasswordValid(string pass, Daemon daemon)
        {
            return PasswordFactory.ComparePasswordsPbkdf2(pass, daemon.password);
        }

        private void FirstLogin(Daemon daemon)
        {
            LogedInDaemon logedInDaemon = new LogedInDaemon() { daemon = daemon, idDaemon = daemon.id, expires = DateTime.Now.AddMinutes(LOGIN_PERIOD) };
            mysql.logedInDaemons.Add(logedInDaemon);
        }

        private LogedInDaemon GetLogedInDaemonWithUuid(Guid uuid)
        { 
            return mysql.logedInDaemons.Where(r => r.idDaemon == mysql.daemons.Where(r2 => r2.uuid == uuid).FirstOrDefault().id).FirstOrDefault();
        }

        /// <summary>
        /// Refreshes for how long a daemon is loged in, throws invalid operation and argument exception
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>true if refreshed timer, false is otherwise</returns>
        public void RefreshLoginTimer(Guid uuid)
        {

            LogedInDaemon logedInDaemon = GetLogedInDaemonWithUuid(uuid);
            if (DateTime.Compare(logedInDaemon.expires, DateTime.Now) < 0 /*Expired*/)
            {
                throw new InvalidOperationException("Je nutno se znova prihlasit");
            }
            if (logedInDaemon != null)
            {
                logedInDaemon.expires = DateTime.Now.AddMinutes(LOGIN_PERIOD);
                mysql.SaveChanges();
            }
            else
                throw new ArgumentException("Daemon with specified uuid not found");
        }

    }
}