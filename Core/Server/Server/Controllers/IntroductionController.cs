using Server.Authentication;
using Server.Models;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Server.Controllers
{
    /// <summary>
    /// Vytváří daemony z preshared klíče a id, registruje vstupní info
    /// </summary>
    public class IntroductionController : ApiController
    {
        private DaemonIntroducer authenticator;
        private MySQLContext mySQL;

        private IntroductionResponse HandleIntroduction(IntroductionMessage message)
        {
            if (authenticator == null)
                authenticator = new DaemonIntroducer((mySQL = new MySQLContext()));

            authenticator.ReadIntroduction(message);
            if (!authenticator.IsValid())
                return new IntroductionResponse() { errorMessage = new ErrorMessage {message="Údaje nejsou platné" } };
            return authenticator.AddToDBMakeResponse();
        }

        /// <summary>
        /// Přijme introduction od daemonu a ověří
        /// </summary>
        /// <param name="value"></param>
        /// <returns>success pokud prošlo, jinak failure</returns>
        public IntroductionResponse Post([FromBody]IntroductionMessage value)
        {
            int i = 0;
            List<MySQLContext> contexts = new List<MySQLContext>();
            
            try
            {
                while (true)
                {
                    var c = new MySQLContext();
                    contexts.Add(c);
                    Thread.Sleep(10);
                    List<Task<int>> tasks = new List<Task<int>>();
                    foreach (var cc in contexts)
                    {
                        var t = new Task<int>(() => cc.Database.ExecuteSqlCommandAsync("SELECT * FROM Daemons").Result);
                        tasks.Add(t);
                    }
                    foreach (var item in tasks)
                    {
                        item.Start();
                    }
                    //Thread.Sleep(10);
                    i++;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine();
            }
            return null;
            //return HandleIntroduction(value);
        }

        protected override void Dispose(bool b)
        {
            //mySQL.Database.Connection.Close();
        }

    }
}