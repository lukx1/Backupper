using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;

namespace Server.Models
{
    /// <summary>
    /// Poskytuje login info schované ve Web.config
    /// </summary>
    /// Pokud mySQL stále nefunguje přidejte nad classu
    /// tento annotation [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public static class MySQLConnectionStringMaker
    {
        /// <summary>
        /// Získá connectionstring schovaná ve Web.config
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return Objects.ConnectionStringHelper.ConnectionString;
        }
    }
}