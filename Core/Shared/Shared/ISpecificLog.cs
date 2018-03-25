using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ISpecificLog<T> : ILog<T>
    {
        LogTable LogTable { get; set; }
        int BoundId { get; set; }
    }
}
