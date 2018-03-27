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
        public Guid Code { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }

        private JsonableUniversalLog() { }

        public static JsonableUniversalLog CreateFrom<T>(ILog<T> log) where T:class
        {
            return new JsonableUniversalLog()
            {
                Id = log.Id,
                Code = log.Code.Uuid,
                Content = JsonConvert.SerializeObject(log.Content),
                DateCreated = log.DateCreated == DateTime.MinValue ? DateTime.Now : log.DateCreated,
                LogType = log.LogType
            };
        }

    }
}
