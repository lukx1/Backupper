using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    public static class StaticUserHelper
    {
        /// <summary>
        /// If he does exists return existing and refreshed session Guid or makes new one.
        /// 
        /// NullReferenceException -> user doesn't exists
        /// InvalidOperationException -> password doesn't match
        /// </summary>
        /// <exception cref="Exceptions.NonExistingUserException"></exception>
        /// <exception cref="Exceptions.NotMatchingPasswordException"></exception>
        /// <returns></returns>
        public static Guid LoginUser(string nickname, string password)
        {
            using (var db = new Models.MySQLContext())
            {
                var user = db.Users.Where(x => x.Nickname == nickname).FirstOrDefault();
                if (user is null)
                    throw new Exceptions.NonExistingUserException();
                if (!Shared.PasswordFactory.ComparePasswordsPbkdf2(password, user.Password))
                    throw new Exceptions.NotMatchingPasswordException();
                return GetValidSessionGuid(user);
            }
        }

        private static Guid GetValidSessionGuid(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                var lgu = db.LogedInUsers.Where(x => x.idUser == user.Id).FirstOrDefault();
                if (lgu is null)
                    return MakeNewSessionGuid(user);
                if (!CheckSessionValidity(lgu.SessionUuid))
                {
                    db.LogedInUsers.Remove(lgu);
                    db.SaveChanges();
                    return MakeNewSessionGuid(user);
                }
                else
                    return lgu.SessionUuid;
            }
        }

        private static Guid MakeNewSessionGuid(Models.User user)
        {
            using (var db = new Models.MySQLContext())
            {
                var lgu = new Models.LogedInUser();
                lgu.idUser = user.Id;
                lgu.SessionUuid = Guid.NewGuid();
                lgu.Expires = DateTime.Now.AddMinutes(15);
                db.LogedInUsers.Add(lgu);
                db.SaveChanges();
                return lgu.SessionUuid;
            }
        }

        public static bool CheckSessionValidity(Guid guid, bool refresh = true)
        {
            using (var db = new Models.MySQLContext())
            {
                var lgu = db.LogedInUsers.Where(x => x.SessionUuid == guid).FirstOrDefault();
                if (lgu is null)
                    return false;
                if (lgu.Expires < DateTime.Now)
                    return false;
                if(refresh)
                {
                    lgu.Expires = DateTime.Now.AddMinutes(15);
                    db.SaveChanges();
                }
                return true;
            }
        }
    }
}