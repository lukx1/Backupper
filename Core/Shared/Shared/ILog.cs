using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Pokud Hází chybu "Type T must be a reference type" tak musíte dopsat where T : class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILog <T> where T : class
    {
        int Id { get;  }
        LogType LogType { get;  }
        LogContentType Code { get;  }
        DateTime DateCreated { get;  }
        T Content { get; }
        void Load(JsonableUniversalLog universalLog);

    }
}
