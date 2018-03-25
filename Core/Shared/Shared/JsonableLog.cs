using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public sealed class JsonableUniversalLog
    {
        public int Id { get; set; }
        public LogType LogType { get; set; }
        public int Code { get; set; }
        public DateTime DateCreated { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }

        private JsonableUniversalLog() { }

        public static JsonableUniversalLog CreateFrom<T>(ILog<T> log)
        {
            if (log is ISpecificLog<T>)
                throw new NotSupportedException("Pro zpracování ISpecificLogu použijte JsonableSpecificLog");
            return new JsonableUniversalLog()
            {
                Id = log.Id,
                Code = (int)log.Code,
                Content = JsonConvert.SerializeObject(log.Content),
                DateCreated = log.DateCreated,
                Header = JsonConvert.SerializeObject(log.Header),
                LogType = log.LogType
            };
        }

    }
}
