using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon.Utility
{
    public interface IConfig
    {
        Guid GetUuid();
        void SetUuid(Guid uuid);

        string GetPass();
        void SetPass(string pass);

        Guid GetSession();
        void SetSession(Guid session);
    }
}
