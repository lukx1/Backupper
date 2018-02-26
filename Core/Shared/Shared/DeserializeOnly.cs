using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    [Obsolete("Metodu nidky nevyužívat, existuje pouze pro deserializaci")]
    class DeserializeOnly : System.Attribute
    {
    }
}
