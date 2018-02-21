using Server.Models;
using Shared;
using Shared.NetMessages;
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


        /// <summary>
        /// Prihlasi daemona, hazi ArgumentException nebo ArgumentNullexception pokud nastane chyba
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="password"></param>
        /// <returns>Uuid</returns>
        public LoginResponse LoginAndGetSessionUuid(Guid uuid, string password)
        {
            Daemon daemon = mysql.daemons.Where(r => r.uuid == uuid).FirstOrDefault();

            if (daemon == null)
                return new LoginResponse() { errorMessage = new ErrorMessage {id = 1,message = "Daemon s daným uuid nebyl nalezen",value=uuid.ToString() } };
            if (!IsPasswordValid(password, daemon))
                return new LoginResponse() { errorMessage = new ErrorMessage { id = 2, message = "Login je neplatný" } };

            LogedInDaemon logedInDaemon = GetLogedInDaemonWithUuid(uuid);

            Guid sessionUuid = Guid.NewGuid();

            if (logedInDaemon == null) { // Daemon existuje ale nikdy nebyl prihlasen
                logedInDaemon = new LogedInDaemon() { daemon = daemon, idDaemon = daemon.id, expires = DateTime.Now.AddMinutes(LOGIN_PERIOD)};
                mysql.logedInDaemons.Add(logedInDaemon);
            }
            else // Daemon existuje a bude prihlasen na loginPrediod
                logedInDaemon.expires = DateTime.Now.AddMinutes(LOGIN_PERIOD);
            logedInDaemon.sessionUuid = sessionUuid;
            mysql.SaveChanges();
            return new LoginResponse() {sessionUuid = sessionUuid };
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