using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.NetMessages.AccessorMessages
{
    /// <summary>
    /// Pro vytváření, upravování a odstraňování přístupových záznamů
    /// </summary>
    public class DbAccessorLog
    {
        public bool IsDaemon;
        /// <summary>
        /// Id záznamu
        /// </summary>
        public int Id;
        /// <summary>
        /// Id daemona nebo usera
        /// </summary>
        ///     Pokud IsDaemon je true potom toto je ID daemona, jinak Usera
        public int IdAccessor;
        public int IdLogType;
        public int Code;
        /// <summary>
        /// Pokud není nastaveno DB si vytvoří vlastní
        /// </summary>
        public DateTime DateCreated;
        public string ShortText;
        public string LongText;
    }
}
