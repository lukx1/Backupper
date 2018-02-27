using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Pro vytváření, upravování a odstraňování userů
    /// </summary>
    ///     Vytváření:
    ///         Nickname 
    ///         FullName 
    ///         Password 
    ///         
    ///     Odstraňování:
    ///         Id
    ///         
    ///     Upravování:
    ///         Id
    ///         Nickname
    ///         FullName
    ///         Password
    ///         
    public class DbUser
    {
        public int Id;
        public string Nickname;
        public string FullName;
        /// <summary>
        /// Pokud v PATCH metode je null tak je nezmenen
        /// </summary>
        public string Password;
    }
}
