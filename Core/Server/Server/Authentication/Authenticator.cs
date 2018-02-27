using Server.Models;
using Shared;
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

        public Guid LoginUser(string nickname, string password)
        {

            User user = mysql.Users.Where(r => r.Nickname == nickname).FirstOrDefault();
            if (user == null)
                throw new NullReferenceException("Uživatel s daným jménem nebyl nalezen");
            if (!PasswordFactory.ComparePasswordsPbkdf2(password, user.Password))
                throw new ArgumentException("Heslo není platné");
            return RealLoginUser(user);
        }

        private Guid RealLoginUser(User user)
        {
            Guid guid = Guid.NewGuid();
            LogedInUser logedInUser = mysql.LogedInUsers.Where(r => r.idUser == user.Id).FirstOrDefault();
            if(logedInUser == null) //First login
            {
                logedInUser = new LogedInUser() { idUser = user.Id, SessionUuid = guid, Expires = DateTime.Now.AddMinutes(15)};
            }
        }

        public Daemon GetDaemonFromUuid(Guid uuid)
        {
            LogedInDaemon logedInDaemon = mysql.LogedInDaemons.Where(r => r.SessionUuid == uuid).FirstOrDefault();
            return mysql.Daemons.Where(r => r.Id == logedInDaemon.IdDaemon).First();
        }

        private void RefreshDaemonSession() { throw new NotImplementedException(); }

        public bool IsSessionValid(Guid uuid, bool refreshTime = true)
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