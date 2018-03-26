using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public sealed class JsonableSpecificLog
    {
        public LogTable LogTable { get; set; }
        public int BoundId { get; set; }
        public int Id { get; set; }
        public LogType LogType { get; set; }
        public int Code { get; set; }
        public DateTime DateCreated { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }

        private JsonableSpecificLog() { }

        public static JsonableSpecificLog CreateFrom<T>(ISpecificLog<T> log)
        {
            return new JsonableSpecificLog()
            {
                Id = log.Id,
                Code = (int)log.Code,
                Content = JsonConvert.SerializeObject(log.Content),
                DateCreated = log.DateCreated,
                Header = JsonConvert.SerializeObject(log.Header),
                LogType =  log.LogType,
                BoundId = log.BoundId,
                LogTable = log.LogTable
            };
        }

    }
}
