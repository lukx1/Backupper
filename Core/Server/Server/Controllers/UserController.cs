using Server.Objects;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    public class UserController : ApiController
    {
        private UserHandler userHandler = new UserHandler();

        /// <summary>
        /// Získá uživatele
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]UserMessage userMessage)
        {
            var users = userHandler.FetchUsers(userMessage);
            if (userHandler.Errors != null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new UserResponse() {ErrorMessages=userHandler.Errors,Users=users });
            else
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new UserResponse() { ErrorMessages = userHandler.Errors, Users = users });
        }

        /// <summary>
        /// Přidá uživatele
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Put([FromBody]UserMessage userMessage)
        {
            userHandler.AddUsers(userMessage);
            if (userHandler.Errors != null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new UserResponse() { ErrorMessages = userHandler.Errors});
            else
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new UserResponse() { ErrorMessages = userHandler.Errors});

        }

        /// <summary>
        /// Upraví uživatele
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Patch([FromBody]UserMessage userMessage)
        {
            userHandler.UpdateUsers(userMessage);
            if (userHandler.Errors != null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new UserResponse() { ErrorMessages = userHandler.Errors });
            else
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new UserResponse() { ErrorMessages = userHandler.Errors });
        }

        /// <summary>
        /// Vymaže uživatele
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete([FromBody]UserMessage userMessage)
        {
            userHandler.DeleteUsers(userMessage);
            if (userHandler.Errors != null)
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.BadRequest, new UserResponse() { ErrorMessages = userHandler.Errors });
            else
                return Util.MakeHttpResponseMessage(System.Net.HttpStatusCode.OK, new UserResponse() { ErrorMessages = userHandler.Errors });
        }
    }
}