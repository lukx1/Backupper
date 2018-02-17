using Server.Models;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    public class DaemonIntroducer
    {
        private IntroductionMessage message;
        private MySQLContext mysql;
        private DaemonPreSharedKey matchingLogin;

        public DaemonIntroducer()
        {
            this.mysql = MySQLContext.Instance;
        }

        /// <summary>
        /// Musí být zavolána první. Načtě zprávu, nidky nehází exceptiony
        /// </summary>
        /// <param name="message"></param>
        public void ReadIntroduction(IntroductionMessage message)
        {
            this.message = message;
        }

        private bool IsCharArraySame(char[] a, char[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        private struct UuidPass
        {
            public string uuid;
            public string pass;
        }
        
        /// <summary>
        /// Vloží data do databáze
        /// </summary>
        /// <param name="dbPreShared"></param>
        /// <returns></returns>
        private UuidPass EnterDBGetPass(DaemonPreSharedKey dbPreShared)
        {
            dbPreShared.used = true;

            var dbDaemonInfo = new DaemonInfo();
            dbDaemonInfo.os = message.os;
            dbDaemonInfo.mac = new string(message.macAdress);

            var dbDaemon = new Daemon();
            var unhashedPass = PasswordFactory.CreateRandomPassword(16);
            var hashedPass = PasswordFactory.HashPasswordPbkdf2(unhashedPass);

            dbDaemon.uuid = Guid.NewGuid();
            dbDaemon.password = hashedPass;
            dbDaemon.idUser = dbPreShared.idUser;
            dbDaemon.daemonInfo = dbDaemonInfo;

            mysql.daemonInfos.Add(dbDaemonInfo);
            mysql.daemons.Add(dbDaemon);

            mysql.SaveChanges();// TODO: make async

            //Console.WriteLine();

            return new UuidPass() { uuid = dbDaemon.uuid.ToString(), pass = unhashedPass};
        }

        /// <summary>
        /// Přidá daemona do databáze a pošle heslo
        /// </summary>
        /// <returns></returns>
        public StandardResponseMessage AddToDBMakeResponse()
        {
            if (matchingLogin == null)
                throw new InvalidOperationException("Can't add to DB if login isn't valid");
            UuidPass uuidPass = EnterDBGetPass(matchingLogin);
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("uuid", uuidPass.uuid);
            values.Add("password", uuidPass.pass);
            StandardResponseMessage successMessage = new StandardResponseMessage() { type=ResponseType.SUCCESS, message = "Added to daemon list",values=values};
            uuidPass.pass = null;
            message = null;
            return successMessage;
        }
        /// 
        /// <summary>
        /// Ověří zda jsou údaje platné
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            
            foreach (var entry in mysql.daemonPreSharedKeys.Where(r => r.id == message.id))
            {
                if (PasswordFactory.ComparePasswordsPbkdf2(message.preSharedKey, entry.preSharedKey))//TODO:Check for expired and used
                {
                    if ((entry.used || DateTime.Compare(entry.expires,DateTime.Now) < 0/*Expired*/) && !Util.IsDebug)
                        continue;
                    matchingLogin = entry;
                    return true;
                }
            }
            return false;
        }

    }
}