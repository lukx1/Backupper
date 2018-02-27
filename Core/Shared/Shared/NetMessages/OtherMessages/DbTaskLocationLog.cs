using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages
{
    /// <summary>
    /// Pro vytváření, upravování a odstraňování zálohovacích záznamů
    /// </summary>
    public class DbTaskLocationLog
    {
        public int Id;
        public int IdTaskLocation;
        public int IdLogType;
        public int Code;
        public DateTime DateCreated;
        public string ShortText;
        public string LongText;
    }
}
