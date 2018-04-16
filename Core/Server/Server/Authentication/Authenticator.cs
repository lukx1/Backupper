using Server.Models;
using Server.Objects;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    /// <summary>
    /// Pro oveřování
    /// </summary>
    public class Authenticator
    {

        public static readonly ErrorMessage BAD_SESSION = new ErrorMessage() { id = 403,message = "Session není platný" };
        public static readonly ErrorMessage EXPIRED_SESSION = new ErrorMessage() { id = 403, message = "Session již vypršel" };

        private MySQLContext mysql;

        /// <summary>
        /// Nutno pokud jiná třída již používá tento kontext
        /// </summary>
        /// <param name="mySQLContext"></param>
        public Authenticator(MySQLContext mySQLContext)
        {
            this.mysql = mySQLContext;
        }

        public Authenticator()
        {
            this.mysql = new MySQLContext();
        }

        /// <summary>
        /// Pokusí se přihlásit uživatele a vrátí uuid
        /// </summary>
        ///     Argument Exception 
        ///         pokud heslo není platné
        ///     NullReference Exception
        ///         pokud uživatel s daným jménem nebyl nalezen
        /// <param name="nickname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Guid LoginUser(string nickname, string password)
        {
            User user = mysql.Users.Where(r => r.Nickname == nickname).FirstOrDefault();
            if (user == null)
                throw new NullReferenceException("Uživatel s daným jménem nebyl nalezen");
            if (!PasswordFactory.ComparePasswordsPbkdf2(password, user.Password))
                throw new ArgumentException("Heslo není platné");
            return GetValidUuid(user);
        }

        /// <summary>
        /// Pomocná třída pro LoginUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Guid GetValidUuid(User user)
        {
            Guid guid = Guid.NewGuid();
            LogedInUser logedInUser = mysql.LogedInUsers.Where(r => r.IdUser == user.Id).FirstOrDefault();
            if (logedInUser == null) //First login
            {
                logedInUser = new LogedInUser() { IdUser = user.Id, SessionUuid = guid, Expires = DateTime.Now.AddMinutes(15) };
                mysql.LogedInUsers.Add(logedInUser);
            }
            else
            {
				logedInUser.SessionUuid = guid;
                logedInUser.Expires = DateTime.Now.AddMinutes(15);
            }
            mysql.SaveChanges(); //DONT CHANGE TO ASYNC
            return guid;
        }

        /// <summary>
        /// Získá z Uuid objekt uživatele
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public User GetUserFromUuid(Guid uuid)
        {
            LogedInUser logedInUsers = mysql.LogedInUsers.Where(r => r.SessionUuid == uuid).FirstOrDefault();
            return mysql.Users.Where(r => r.Id == logedInUsers.IdUser).FirstOrDefault();
        }

        /// <summary>
        /// Vracé z Uuid objekt daemona
        /// </summary>
        /// <param name="sessionUuid"></param>
        /// <returns></returns>
        public Daemon GetDaemonFromUuid(Guid sessionUuid)
        {
            LogedInDaemon logedInDaemon = mysql.LogedInDaemons.Where(r => r.SessionUuid == sessionUuid).FirstOrDefault();
            return mysql.Daemons.Where(r => r.Id == logedInDaemon.IdDaemon).FirstOrDefault();
        }

        public struct UuidPass
        {
            public string name;
            public string pass;
        }

        /// <summary>
        /// Introducne daemona
        /// </summary>
        /// <param name="preSharedKey"></param>
        /// <param name="os"></param>
        /// <param name="mac"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public UuidPass IntroduceDaemon(DaemonPreSharedKey preSharedKey, string os, string mac, int idUser, char[] pubKey)
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

        public IList<Permission> GetPermissionsDaemon(Guid daemonUuid)
        {
            /* var perms = from groupPermissions in mysql.GroupPermissions
                         join ppermissions in mysql.Permissions on groupPermissions.IdPermission equals ppermissions.Id
                         join daemonGroups in mysql.DaemonGroups on groupPermissions.IdGroup equals daemonGroups.IdGroup
                         join daemons in mysql.Daemons on daemonGroups.IdDaemon equals daemons.Id
                         where daemons.Uuid == uuid
                         select ppermissions.Id;
             List<Permission> permissions = new List<Permission>();
             foreach (var perm in perms)
             {
                 permissions.Add((Permission)perm);
             }
             return permissions;*/
            PList permissionsList = new PList();
            var permissions = from daemons in mysql.Daemons
                              join daemonGroups in mysql.DaemonGroups on daemons.Id equals daemonGroups.IdDaemon
                              join groups in mysql.Groups on daemonGroups.IdGroup equals groups.Id
                              join groupPermissions in mysql.GroupPermissions on groups.Id equals groupPermissions.IdGroup
                              where daemons.Uuid == daemonUuid
                              select new { groupPermissions.IdPermission };
            foreach (var permission in permissions)
            {
                permissionsList.Add((Permission)permission.IdPermission);
                if ((Permission)permission.IdPermission == Permission.SKIP)
                    break;
            }
            return permissionsList;
        }

        /// <summary>
        /// Zjistí jestli daemon s daným uuid může udělat danou věc
        /// </summary>
        /// <param name="daemonUuid">Daemonovo uuid</param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool IsDaemonAllowed(Guid daemonUuid, Server.Authentication.Permission permission)
        {

            var permissions = from daemons in mysql.Daemons
                      join daemonGroups in mysql.DaemonGroups on daemons.Id equals daemonGroups.IdDaemon
                      join groups in mysql.Groups on daemonGroups.IdGroup equals groups.Id
                      join groupPermissions in mysql.GroupPermissions on groups.Id equals groupPermissions.IdGroup
                      where daemons.Uuid == daemonUuid && (groupPermissions.IdPermission == (int)permission || groupPermissions.IdPermission == (int)Permission.SKIP)
                      select new { groupPermissions.IdPermission };
            return permissions.Count() > 0;
            /*var daemon = mysql.Daemons.Where(r => r.Uuid == daemonUuid).FirstOrDefault();
            if (daemon == null)
                throw new NullReferenceException("Nelze najít daemona s daným uuid");
            var daemonGroups = mysql.DaemonGroups.Where(r => r.IdDaemon == daemon.Id);
            foreach (var dGroups in daemonGroups)
            {
                var groups = mysql.Groups.Where(r => r.Id == dGroups.IdGroup);
                foreach (var group in groups)
                {
                    var groupPermissions = mysql.GroupPermissions.Where(r => r.IdGroup == dGroups.Id);
                    foreach (var groupPermission in groupPermissions)
                    {
                        if (groupPermission.IdPermission == (int)permission)
                            return true;
                        else if (groupPermission.IdPermission == (int)Permission.SKIP)
                            return true;
                    }
                }
                
            }
            return false;*/
        }

        /// <summary>
        /// Zjistí jestli daemon s daným uuid může udělat danou věc
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Zjistí všechny skupiny uživatele z jeho nicku
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public IEnumerable<Models.Group> GetGroupsFromUserNickname(string nickname)
        {
            return GetGroupsFromUserId(mysql.Users.FirstOrDefault(x => x.Nickname == nickname).Id);
        }

        /// <summary>
        /// Zjistí všechny skupiny uživatele z jeho Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Models.Group> GetGroupsFromUserId(int id)
        {
            return (from userGroups in mysql.UserGroups
                join groups in mysql.Groups on userGroups.IdGroup equals groups.Id
                where userGroups.IdUser == id
                select groups).ToArray();
        }

        /// <summary>
        /// Zjistí všechny permisse uživatele z jeho nicku
        /// Pokud má uživatel Permission.SKIP tak se vrátí kolekce pouze s tímto prvkem
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public IEnumerable<Permission> GetPermissionsFromUserNickname(string nickname)
        {
            return GetPermissionsFromUserId(mysql.Users.FirstOrDefault(x => x.Nickname == nickname).Id);
        }

        /// <summary>
        /// Zjistí všechny permisse uživatele z jeho Id
        /// Pokud má uživatel Permission.SKIP tak se vrátí kolekce pouze s tímto prvkem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Permission> GetPermissionsFromUserId(int id)
        {
            Permission[] permissions = (from userGroups in mysql.UserGroups
                join groups in mysql.Groups on userGroups.IdGroup equals groups.Id
                join groupPermissions in mysql.GroupPermissions on groups.Id equals groupPermissions.IdGroup
                where userGroups.IdUser == id
                select (Permission)groupPermissions.IdPermission).ToArray();
            return permissions.Contains(Permission.SKIP) ? new[] {Permission.SKIP} : permissions;
        }

        /// <summary>
        /// Zjístí předzdílen=y klíč s daným ID, toto ID není ID uživatele
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DaemonPreSharedKey GetPresharedFromId(int id)
        {
            return mysql.DaemonPreSharedKeys.Where(r => r.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Zjistí jestli předzdílený klíč je platný
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsPresharedValid(int id, string key)
        {
            var preshared = mysql.DaemonPreSharedKeys.Where(r => r.Id == id).FirstOrDefault();
            if (preshared == null)
                return false;
            if (preshared.Used || DateTime.Compare(preshared.Expires, DateTime.Now) < 0 /*Expired*/)
                return false;
            return PasswordFactory.ComparePasswordsPbkdf2(key, preshared.PreSharedKey);
        }

        /// <summary>
        /// Zjistí zda sezení je platné
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="IsDaemon"></param>
        /// <param name="refreshTime"></param>
        /// <returns></returns>
        public bool IsSessionValid(Guid uuid, bool IsDaemon, bool refreshTime = true)
        {
            if (IsDaemon)
                return IsDaemonSessionValid(uuid, refreshTime);
            else
                return IsUserSessionValid(uuid, refreshTime);
        }

        /// <summary>
        /// Zjistí zda sezení usera je platné
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="refreshTime"></param>
        /// <returns></returns>
        public bool IsUserSessionValid(Guid uuid, bool refreshTime = true)
        {
            LogedInUser logedInUser = mysql.LogedInUsers.Where(r => r.SessionUuid == uuid).FirstOrDefault();
            if (logedInUser == null)
                return false;
            if (Util.IsExpired(logedInUser.Expires))
                return false;
            logedInUser.Expires = DateTime.Now.AddMinutes(15);
            mysql.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Zjistí zda sezení daemona je platné
        /// </summary>
        /// <param name="sessionUuid"></param>
        /// <param name="refreshTime"></param>
        /// <returns></returns>
        public bool IsDaemonSessionValid(Guid sessionUuid, bool refreshTime = true)
        {
            LogedInDaemon logedInDaemon = mysql.LogedInDaemons.Where(r => r.SessionUuid == sessionUuid).FirstOrDefault();
            if (logedInDaemon == null)
                return false;
            if (Util.IsExpired(logedInDaemon.Expires))
                return false;
            logedInDaemon.Expires = DateTime.Now.AddMinutes(15);
            mysql.SaveChangesAsync();
            return true;
        }
    }
}