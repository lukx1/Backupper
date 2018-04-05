using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Tools -> Create Guid
    /// </summary>
    ///  9D58460A-23B7-48FB-B157-85AA74E6F9B1
    ///  CB93C3C5-9326-439C-82A3-783EE179ABE0
    ///  D938191C-4320-4F8B-800F-513F9AC1EED3
    ///  
    ///  
    public sealed class LogContentType
    {
        public static readonly LogContentType DEBUG = new LogContentType("F85A5D8F-6A99-48E6-B5C6-9D14DE7FE9BF");
        public static readonly LogContentType SERVER_STATUS = new LogContentType("BE588A03-F535-49A6-9BDD-F5E48BAC08CC");
        public static readonly LogContentType DAEMON_CRASH = new LogContentType("11008931-81D3-4D12-81B9-2C09DD6548E2");

        public Guid Uuid { get; private set; }

        private LogContentType(string guid)
        {
            this.Uuid = new Guid(guid);

        }
}
}
