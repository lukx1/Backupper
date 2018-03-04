using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class DbLocationCredential
    {
        /// <summary>
        /// Id LC, při vytváření nemusí být nastaveno
        /// </summary>
        public int Id;

        /// <summary>
        /// Host lokace 
        /// </summary>
        public string host;

        /// <summary>
        /// Port
        /// </summary>
        public int port;

        /// <summary>
        /// Způsob připojení
        /// </summary>
        public DbLogonType LogonType;


        public string username;
        public string password;
    }
}
