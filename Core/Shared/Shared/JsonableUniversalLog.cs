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
        public int Id { get; private set; }
        public LogType LogType { get; private set; }
        public Guid Code { get; private set; }
        public DateTime DateCreated { get; private set; }
        public string Content { get; private set; }

        private JsonableUniversalLog() { }


        public static JsonableUniversalLog CreateFrom(int Id, int LogType, Guid Code, DateTime DateCreated, string Content)
        {
            return new JsonableUniversalLog()
            {
                Id = Id,
                LogType = (LogType)LogType,
                Code = Code,
                Content = Content,
                DateCreated = DateCreated,
            };
        }

        public static TContent ParseContent<TContent>(string Content)
        {
            return JsonConvert.DeserializeObject<TContent>(Content);
        }

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
