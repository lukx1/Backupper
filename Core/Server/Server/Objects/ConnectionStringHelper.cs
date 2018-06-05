using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Web.Configuration;

namespace Server.Objects
{
    /// <summary>
    /// Dynamicky ziska nebo nastavi connectionString
    /// </summary>
    public static class ConnectionStringHelper
    {
        public static string ConnectionString
        {
            get => WebConfigurationManager.ConnectionStrings["BCS"]?.ConnectionString;
            set
            {
                var conf = WebConfigurationManager.OpenWebConfiguration("~");
                var csss = (ConnectionStringsSection)conf.GetSection("connectionStrings");
                var css = new ConnectionStringSettings("BCS", value);
                csss.ConnectionStrings.Clear();
                csss.ConnectionStrings.Add(css);
                conf.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}