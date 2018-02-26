using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.TaskMessages
{
    public class Protocol
    {
        public static readonly Protocol WND = new Protocol(1, "WND", "Windows Standard URI");
        public static readonly Protocol WRD = new Protocol(2, "WRD", "Windows Remote URI");
        public static readonly Protocol FTP = new Protocol(3, "FTP", "File Transfer Protocol");
        public static readonly Protocol SFTP = new Protocol(4, "SFTP", "Secure File Transfer Protocol");

        [DeserializeOnly]
        public Protocol()
        {

        }

        private Protocol(int id, string shortName, string longName)
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
