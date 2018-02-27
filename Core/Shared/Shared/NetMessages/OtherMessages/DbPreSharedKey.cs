using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Pro vytváření, upravování a odstraňování klíčů
    /// </summary>
    public class DbPreSharedKey
    {
        public int Id;
        public int IdUser;
        public string PerSharedKey;
        public DateTime Expires;
        public bool Used;
    }
}
