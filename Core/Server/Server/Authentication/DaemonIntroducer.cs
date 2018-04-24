using Server.Models;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using static Server.Authentication.Authenticator;

namespace Server.Authentication
{
    public class DaemonIntroducer
    {
        private IntroductionMessage message;
        private MySQLContext mysql;
        private DaemonPreSharedKey matchingLogin;
        private Authenticator authenticator;

        public DaemonIntroducer(MySQLContext mysql)
        {
            this.mysql = mysql;
            authenticator = new Authenticator(mysql);
        }

        /// <summary>
        /// Musí být zavolána první. Načtě zprávu, nidky nehází exceptiony
        /// </summary>
        /// <param name="message"></param>
        public void ReadIntroduction(IntroductionMessage message)
        {
            this.message = message;
        }

        /// <summary>
        /// Vloží data do databáze
        /// </summary>
        /// <param name="dbPreShared"></param>
        /// <returns></returns>
        private UuidPass EnterDBGetPass(DaemonPreSharedKey dbPreShared)
        {

            return authenticator.IntroduceDaemon(dbPreShared,message.os,new string(message.macAdress),dbPreShared.IdUser,message.publicKey,message.PCUuid);
        }

        /// <summary>
        /// Přidá daemona do databáze a pošle heslo
        /// </summary>
        /// <returns></returns>
        public IntroductionResponse AddToDBMakeResponse()
        {
            if (matchingLogin == null)
            {
                var resp = new IntroductionResponse { ErrorMessages = new List<ErrorMessage>() };
                resp.ErrorMessages.Add(new ErrorMessage() { id=(int)HttpStatusCode.BadRequest, message = "Login se neshoduje", value = "login" });
                return resp;
            }
            UuidPass uuidPass = EnterDBGetPass(matchingLogin);
            IntroductionResponse response = new IntroductionResponse { uuid = new Guid(uuidPass.name), password = uuidPass.pass, ErrorMessages = new List<ErrorMessage>() };
            uuidPass.pass = null;
            message = null;
            return response;
        }

        

        /// 
        /// <summary>
        /// Ověří zda jsou údaje platné
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {

            if (authenticator.IsPresharedValid(message.id, message.preSharedKey))
            {
                matchingLogin = authenticator.GetPresharedFromId(message.id);
                return true;
            }
            else
                return false;
        }

    }
}