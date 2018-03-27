using Server.Models;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Server.Authentication
{
    public class DaemonLoginer
    {
        //TODO: Udelat soucasti authenticatoru
        private MySQLContext mysql;
        /// <summary>
        /// Login extension period in minutes
        /// </summary>
        public const long LOGIN_PERIOD = 15;

        public DaemonLoginer()
        {
            mysql = new MySQLContext();
        }

        private LoginResponse LoginErrMaker(HttpStatusCode code, string msg, string val = null)
        {
            LoginResponse response = new LoginResponse();
            response.ErrorMessages = new List<ErrorMessage>();
            response.ErrorMessages.Add(new ErrorMessage() { id = (int)code, message = msg, value = val });
            return response;
        }



        /// <summary>
        /// Prihlasi daemona, hazi ArgumentException nebo ArgumentNullexception pokud nastane chyba
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="password"></param>
        /// <returns>Uuid</returns>
        public LoginResponse LoginAndGetSessionUuid(Guid uuid, string password)
        {
            Daemon daemon = mysql.Daemons.Where(r => r.Uuid == uuid).FirstOrDefault();

            if (daemon == null)
                return LoginErrMaker(HttpStatusCode.NotFound, "Daemon s daným uuid nebyl nalezen", uuid.ToString());
            if (!IsPasswordValid(password, daemon))
                return LoginErrMaker(HttpStatusCode.Forbidden, "Login je neplatný");

            Authenticator authenticator = new Authenticator();

            if (!authenticator.IsDaemonAllowed(daemon.Uuid,Permission.LOGIN))
                return LoginErrMaker(HttpStatusCode.Forbidden, "Daemon nemá pravomoce se přihlásit");

            LogedInDaemon logedInDaemon = GetLogedInDaemonWithUuid(uuid);

            Guid sessionUuid = Guid.NewGuid();

            if (logedInDaemon == null) { // Daemon existuje ale nikdy nebyl prihlasen
                logedInDaemon = new LogedInDaemon() { Daemon = daemon, IdDaemon = daemon.Id, Expires = DateTime.Now.AddMinutes(LOGIN_PERIOD)};
                mysql.LogedInDaemons.Add(logedInDaemon);
            }
            else // Daemon existuje a bude prihlasen na loginPrediod
                logedInDaemon.Expires = DateTime.Now.AddMinutes(LOGIN_PERIOD);
            logedInDaemon.SessionUuid = sessionUuid;
            mysql.SaveChanges();
            return new LoginResponse() {sessionUuid = sessionUuid };
        }

        private bool IsPasswordValid(string pass, Daemon daemon)
        {
            return PasswordFactory.ComparePasswordsPbkdf2(pass, daemon.Password);
        }


        private void FirstLogin(Daemon daemon)
        {
            LogedInDaemon logedInDaemon = new LogedInDaemon() { Daemon = daemon, IdDaemon = daemon.Id, Expires = DateTime.Now.AddMinutes(LOGIN_PERIOD) };
            mysql.LogedInDaemons.Add(logedInDaemon);
        }

        private LogedInDaemon GetLogedInDaemonWithUuid(Guid uuid)
        { 
            return mysql.LogedInDaemons.Where(r => r.IdDaemon == mysql.Daemons.Where(r2 => r2.Uuid == uuid).FirstOrDefault().Id).FirstOrDefault();
        }



    }
}