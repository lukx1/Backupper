using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Server.Models
{
    public static class MySQLConnectionStringMaker
    {
        public static string GetConnectionString()
        {
            var ini = new SQLConnectionIni();
            return $@"server={ini.Server};persistsecurityinfo=True;database={ini.Database};User ID={ini.UserID};password={ini.Password};";
        }
    }
}