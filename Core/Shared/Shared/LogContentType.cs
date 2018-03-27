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
    public class LogContentType
    {
        public static readonly LogContentType DEBUG = new LogContentType("F85A5D8F-6A99-48E6-B5C6-9D14DE7FE9BF");

        public Guid Uuid { get; private set; }

        private LogContentType(string guid)
        {
            this.Uuid = new Guid(guid);

        }
}
}
