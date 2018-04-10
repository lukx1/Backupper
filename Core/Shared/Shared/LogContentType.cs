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
    ///  {16FE7561-5DB1-413B-BB91-06EA98F980C4}
    ///  {0940828A-363F-4B09-8C94-1F8733F2B28D}
    ///  {C120E029-5D7E-4FBD-B76D-8B7052B311A8}
    ///  {}
    ///  {}
    public sealed class LogContentType
    {
        public static readonly LogContentType DEBUG = new LogContentType("F85A5D8F-6A99-48E6-B5C6-9D14DE7FE9BF");
        public static readonly LogContentType SERVER_STATUS = new LogContentType("BE588A03-F535-49A6-9BDD-F5E48BAC08CC");
        public static readonly LogContentType DAEMON_CRASH = new LogContentType("11008931-81D3-4D12-81B9-2C09DD6548E2");
        public static readonly LogContentType DAEMON_FAILED_LOGIN = new LogContentType("D938191C-4320-4F8B-800F-513F9AC1EED3");
        public static readonly LogContentType DAEMON_FAILED_INTRO = new LogContentType("CB93C3C5-9326-439C-82A3-783EE179ABE0");
        public static readonly LogContentType DAEMON_FAILED_TASK_GENERAL = new LogContentType("9D58460A-23B7-48FB-B157-85AA74E6F9B1");
        public static readonly LogContentType DAEMON_GENERAL_SERVER_RESPONSE = new LogContentType("5BB6A5E2-0266-48E9-8E7A-E1B55455613E");
        public static readonly LogContentType DAEMON_GENERAL_ERROR = new LogContentType("A1374316-A7EC-43E0-B2C7-187FCEAF5D41");

        public Guid Uuid { get; private set; }

        private LogContentType(string guid)
        {
            this.Uuid = new Guid(guid);

        }
}
}
