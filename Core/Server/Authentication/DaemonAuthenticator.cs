using Server.Models;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Authentication
{
    public class DaemonAuthenticator
    {
        private IntroductionMessage message;
        private DaemonLogin daemonLogin;
        private daemonPreSharedKey matchingLogin;

        public DaemonAuthenticator(DaemonLogin daemonLogin)
        {
            this.daemonLogin = daemonLogin;
        }

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

        

        private string EnterDBGetPass(daemonPreSharedKey dbPreShared)
        {
            dbPreShared.used = true;

            var dbDaemonInfo = new daemonInfo();
            dbDaemonInfo.os = message.os;
            dbDaemonInfo.mac = new string(message.macAdress);

            var dbDaemon = new daemon();
            var unhashedPass = PasswordFactory.CreateRandomPassword(16);
            var hashedPass = PasswordFactory.HashPasswordPbkdf2(unhashedPass);

            dbDaemon.password = hashedPass;
            dbDaemon.idUser = dbPreShared.idUser;
            dbDaemon.daemonInfo = dbDaemonInfo;

            daemonLogin.SaveChanges();// TODO: make async
            daemonLogin.Dispose();
            return unhashedPass;
        }

        /// <summary>
        /// Closes DB connection
        /// </summary>
        /// <returns></returns>
        public INetMessage AddToDBMakeResponse()
        {
            if (matchingLogin == null)
                throw new InvalidOperationException("Can't add to DB if login isn't valid");
            var unhashedpass = EnterDBGetPass(matchingLogin);
            SuccessMessage successMessage = new SuccessMessage() { message = "Added to daemon list",value = unhashedpass };
            unhashedpass = null; // Clean RAM
            message = null;
            return successMessage;
        }

        public bool IsValid()
        {
            foreach (var entry in daemonLogin.daemonPreSharedKeys)
            {
                if (PasswordFactory.ComparePasswordsPbkdf2(message.preSharedKey, entry.preSharedKey))//TODO:Check for expired and used
                {
                    matchingLogin = entry;
                    return true;
                }
            }
            return false;
        }

    }
}