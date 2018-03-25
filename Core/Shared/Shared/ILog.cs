using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ILog <T>
    {
        int Id { get;  }
        LogType LogType { get;  }
        LogContentType Code { get;  }
        DateTime DateCreated { get;  }
        ILogHeader Header { get;  }
        T Content { get; }
    }
}
