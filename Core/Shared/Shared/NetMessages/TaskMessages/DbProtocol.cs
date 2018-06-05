using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class DbProtocol
    {
        public static readonly DbProtocol WND = new DbProtocol(1, "WND", "Windows Standard URI");
        public static readonly DbProtocol WRD = new DbProtocol(2, "WRD", "Windows Remote URI");
        public static readonly DbProtocol FTP = new DbProtocol(3, "FTP", "File Transfer Protocol");
        public static readonly DbProtocol SFTP = new DbProtocol(4, "SFTP", "Secure File Transfer Protocol");
        public static readonly DbProtocol MYSQL = new DbProtocol(5, "MYSQL", "MySQL");

        [DeserializeOnly]
        public DbProtocol()
        {

        }

        public static bool operator !=(DbProtocol a, DbProtocol b)
        {
            return a.Id != b.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is DbProtocol)
                return ((DbProtocol)obj).Id == this.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public static bool operator ==(DbProtocol a, DbProtocol b)
        {
            return a.Id == b.Id;
        }

        private DbProtocol(int id, string shortName, string longName)
        {
            this.Id = id;
            this.ShortName = shortName;
            this.LongName = longName;
        }

        [DeserializeOnly]
        public int Id;

        [DeserializeOnly]
        public string ShortName;

        [DeserializeOnly]
        public string LongName;
    }
}
