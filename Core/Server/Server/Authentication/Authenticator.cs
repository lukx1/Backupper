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

        public Authenticator(MySQLContext mySQLContext)
        {
            this.mysql = mySQLContext;
        }

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
            return CreateUuid(user);
        }

        private Guid CreateUuid(User user)
        {
            Guid guid = Guid.NewGuid();
            LogedInUser logedInUser = mysql.LogedInUsers.Where(r => r.idUser == user.Id).FirstOrDefault();
            if(logedInUser == null) //First login
            {
                logedInUser = new LogedInUser() { idUser = user.Id, SessionUuid = guid, Expires = DateTime.Now.AddMinutes(15)};
                mysql.LogedInUsers.Add(logedInUser);
            }
            else
            {
                logedInUser.Expires = DateTime.Now.AddMinutes(15);
            }
            mysql.SaveChanges(); //DONT CHANGE TO ASYNC
            return guid;
        }

        public User GetUserFromUuid(Guid uuid)
        {
            LogedInUser logedInUsers = mysql.LogedInUsers.Where(r => r.SessionUuid == uuid).FirstOrDefault();
            return mysql.Users.Where(r => r.Id == logedInUsers.idUser).FirstOrDefault();
        }

        public Daemon GetDaemonFromUuid(Guid uuid)
        {
            LogedInDaemon logedInDaemon = mysql.LogedInDaemons.Where(r => r.SessionUuid == uuid).FirstOrDefault();
            return mysql.Daemons.Where(r => r.Id == logedInDaemon.IdDaemon).FirstOrDefault();
        }

        public struct UuidPass
        {
            public string name;
            public string pass;
        }

        public UuidPass IntroduceDaemon(DaemonPreSharedKey preSharedKey, string os, string mac, int idUser)
        {
            preSharedKey.Used = true;

            var dbDaemonInfo = new DaemonInfo();
            dbDaemonInfo.Os = os;
            dbDaemonInfo.Mac = mac;

            var dbDaemon = new Daemon();
            var unhashedPass = PasswordFactory.CreateRandomPassword(16);
            var hashedPass = PasswordFactory.HashPasswordPbkdf2(unhashedPass);

            dbDaemon.Uuid = Guid.NewGuid();
            dbDaemon.Password = hashedPass;
            dbDaemon.IdUser = idUser;
            dbDaemon.DaemonInfo = dbDaemonInfo;

            mysql.DaemonInfos.Add(dbDaemonInfo);
            mysql.Daemons.Add(dbDaemon);

            mysql.DaemonGroups.Add(new DaemonGroup() { Daemon = dbDaemon, IdGroup = (int)Group.DAEMONS }); // Ads to default group

            mysql.SaveChanges();

            return new UuidPass() { name = dbDaemon.Uuid.ToString(), pass = unhashedPass };
        }

        public bool IsDaemonAllowed(Guid uuid, Server.Authentication.Permission permission)
        {
            var daemon = mysql.Daemons.Where(r => r.Uuid == uuid).FirstOrDefault();
            if (daemon == null)
                return false;
            var groups = mysql.DaemonGroups.Where(r => r.IdDaemon == daemon.Id);
            foreach (var group in groups)
            {
                var groupPermissions = mysql.GroupPermissions.Where(r => r.IdGroup == group.Id);
                foreach (var groupPermission in groupPermissions)
                {
                    if (groupPermission.IdPermission == (int)permission)
                        return true;
                    else if (groupPermission.IdPermission == (int)Permission.SKIP)
                        return true;
                }
            }
            return false;
        }

        public bool IsUserAllowed(string nickname, Server.Authentication.Permission permission)
        {
            var user = mysql.Users.Where(r => r.Nickname == nickname).FirstOrDefault();
            if (user == null)
                return false;
            var groups = mysql.DaemonGroups.Where(r => r.IdDaemon == user.Id);
            foreach (var group in groups)
            {
                var groupPermissions = mysql.GroupPermissions.Where(r => r.IdGroup == group.Id);
                foreach (var groupPermission in groupPermissions)
                {
                    if (groupPermission.IdPermission == (int)permission)
                        return true;
                    else if (groupPermission.IdPermission == (int)Permission.SKIP)
                        return true;
                }
            }
            return false;
        }

        public DaemonPreSharedKey GetPresharedFromId(int id)
        {
            return mysql.DaemonPreSharedKeys.Where(r => r.Id == id).FirstOrDefault();
        }

        public bool IsPresharedValid(int id,string key)
        {
            var preshared = mysql.DaemonPreSharedKeys.Where(r => r.Id == id).FirstOrDefault();
            if (preshared == null)
                return false;
            if (preshared.Used || DateTime.Compare(preshared.Expires, DateTime.Now) < 0 /*Expired*/)
                return false;
            return PasswordFactory.ComparePasswordsPbkdf2(key, preshared.PreSharedKey);
        }

        public bool IsSessionValid(Guid uuid,bool IsDaemon, bool refreshTime = true)
        {
            if (IsDaemon)
            {
                LogedInDaemon logedInDaemon = mysql.LogedInDaemons.Where(r => r.SessionUuid == uuid).FirstOrDefault();
                if (logedInDaemon == null)
                    return false;
                if (Util.IsExpired(logedInDaemon.Expires))
                    return false;
                return true;
            }
            else
            {
                LogedInUser logedInUser = mysql.LogedInUsers.Where(r => r.SessionUuid == uuid).FirstOrDefault();
                if (logedInUser == null)
                    return false;
                if (Util.IsExpired(logedInUser.Expires))
                    return false;
                return true;
            }
        }

    }
}