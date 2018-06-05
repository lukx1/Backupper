using Server.Authentication;
using Server.Models;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Objects
{ 
    /// <summary>
    /// Repozitar pro DbUser
    /// </summary>
    public class UserHandler
    {
        private MySQLContext mysql;
        private List<ErrorMessage> errors = new List<ErrorMessage>();
        private Authenticator authenticator = new Authenticator();
        public List<ErrorMessage> Errors { get => errors; }

        public UserHandler()
        {
            mysql = new MySQLContext();
        }

        private DbUser FetchUser(string nick)
        {
            var user = mysql.Users.Where(r => r.Nickname == nick).FirstOrDefault();
            if (user == null)
                throw new NullReferenceException("Uzivatel s danym nicknamem nebyl nalezen");
            return new DbUser() { FullName = user.FullName, Id = user.Id, Nickname = user.Nickname, Password = user.Password };
        }

        
        private List<DbUser> FetchAll()
        {
            List<DbUser> dbUsers = new List<DbUser>();
            var users = mysql.Users;
            foreach (var user in users)
            {
                dbUsers.Add(new DbUser() {FullName = user.FullName,Id=user.Id,Nickname=user.Nickname,Password=user.Password });
            }
            return dbUsers;
        }

        private bool CanFetchUsers(UserMessage userMessage)
        {
            User user = authenticator.GetUserFromUuid(userMessage.sessionUuid);
            if (!authenticator.IsSessionValid(userMessage.sessionUuid, false))
                return false;
            foreach (var users in userMessage.Users)
            {
                if (users.Nickname != user.Nickname)
                    return authenticator.IsUserAllowed(user.Nickname, Authentication.Permission.MANAGEOTHERUSERS);
            }
            return authenticator.IsUserAllowed(user.Nickname, Authentication.Permission.MANAGESELFUSER);
        }

        private bool CanAddUsers(UserMessage userMessage)
        {
            if (!authenticator.IsSessionValid(userMessage.sessionUuid, false))
                return false;
            var user = authenticator.GetUserFromUuid(userMessage.sessionUuid);
            return authenticator.IsUserAllowed(user.Nickname, Authentication.Permission.MANAGEOTHERUSERS);
        }

        private bool CanUpdateUsers(UserMessage userMessage)
        {
            if (!authenticator.IsSessionValid(userMessage.sessionUuid, false))
                return false;
            var user = authenticator.GetUserFromUuid(userMessage.sessionUuid);
            return authenticator.IsUserAllowed(user.Nickname, Authentication.Permission.MANAGEOTHERUSERS);
        }

        private bool CanDeleteUsers(UserMessage userMessage)
        {
            if (!authenticator.IsSessionValid(userMessage.sessionUuid, false))
                return false;
            var user = authenticator.GetUserFromUuid(userMessage.sessionUuid);
            return authenticator.IsUserAllowed(user.Nickname, Authentication.Permission.MANAGEOTHERUSERS);
        }

        public void UpdateUsers(UserMessage userMessage)
        {
            errors.Clear();

            if (!CanUpdateUsers(userMessage))
            {
                errors.Add(new ErrorMessage() { id = -1, message = "Tato akce neni dovolena", value = "*" });
                return;
            }

            try
            {
                foreach (var dbUser in userMessage.Users) // Prepsat na for
                {
                    var user = mysql.Users.Where(r => r.Nickname == dbUser.Nickname).FirstOrDefault();
                    if (user == null)
                    {
                        errors.Add(new ErrorMessage() { id = 3, message = "Uzivatel " + dbUser.Nickname + " nebyl nalezen", value = dbUser.Nickname });
                        continue;
                    }
                    user.FullName = dbUser.FullName;
                    user.Nickname = dbUser.Nickname;
                    if (dbUser.Password != null)
                        dbUser.Password = PasswordFactory.HashPasswordPbkdf2(dbUser.Password);
                }
                mysql.SaveChanges();
            }
            catch(Exception e)
            {
                errors.Add(new ErrorMessage() { id = e.HResult, message = e.Message, value = "-1" });
            }
        }

        public void DeleteUsers(UserMessage userMessage)
        {
            errors.Clear();

            if (!CanDeleteUsers(userMessage))
            {
                errors.Add(new ErrorMessage() { id = -1, message = "Tato akce neni dovolena", value = "*" });
                return;
            }

            try
            {
                for (int i = 0; i < userMessage.Users.Count; i++)
                {
                    var dbUser = userMessage.Users[i];
                    if (dbUser.Nickname == "Server")
                        continue;
                    mysql.Users.Remove(new User() {Nickname=dbUser.Nickname });
                }
                mysql.SaveChanges();
            }
            catch (Exception e)
            {
                errors.Add(new ErrorMessage() { id = e.HResult, message = e.Message, value = "-1" });
            }
        }

        public void AddUsers(UserMessage userMessage)
        {
            errors.Clear();

            if (!CanAddUsers(userMessage))
            {
                errors.Add(new ErrorMessage() { id=-1,message="Tato akce neni dovolena",value="*" });
                return;
            }

            try
            {
                for (int i = 0; i < userMessage.Users.Count; i++)
                {
                    var dbUser = userMessage.Users[i];
                    if (dbUser.Password == null)
                    {
                        errors.Add(new ErrorMessage() { id = 1, message = "Heslo nesmí být null", value = i.ToString() });
                        continue;
                    }

                    User user = new User()
                    {
                        Nickname = dbUser.Nickname,
                        FullName = dbUser.FullName,
                        Password = PasswordFactory.HashPasswordPbkdf2(dbUser.Password)
                    };
                    mysql.Users.Add(user);
                }
                mysql.SaveChanges();
            }
            catch(Exception e)
            {
                errors.Add(new ErrorMessage() { id = e.HResult, message = e.Message, value = "-1" });
            }
        }

        public List<DbUser> FetchUsers(UserMessage userMessage)
        {
            errors.Clear();

            if (!CanFetchUsers(userMessage)) // Ma dovoleno toto udelat
            {
                errors.Add(new ErrorMessage() { id = -1, message = "Tato akce není dovolena", value = "*" });
                return new List<DbUser>();
            }

            if (userMessage.Users.Count == 0) // Chce vsechno
                return FetchAll();

            int i = 0;
            List<DbUser> users = new List<DbUser>();
            foreach (var user in userMessage.Users) // Chce specificka jmena
            {
                try
                {
                    users.Add(FetchUser(user.Nickname));
                }
                catch(Exception e)
                {
                    errors.Add(new ErrorMessage() { id = e.HResult,message = e.Message, value =i.ToString()});
                }
                i++;//SRY
            }
            return users;
        }

    }
}