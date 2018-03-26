using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    public class SimpleHeader : ILogHeader
    {
        public string Binder { get; set; } = null;
        public int Page { get; set; } = 1;
        public int Pages { get; set; } = 1;
    }
}
