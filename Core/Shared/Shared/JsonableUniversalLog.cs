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

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        [Serializable]
        private class CarryJUL
        {
            public int Id { get; set; }
            public LogType LogType { get; set; }
            public Guid Code { get; set; }
            public DateTime DateCreated { get; set; }
            public string Content { get; set; }
        }

        public static JsonableUniversalLog FromJson(string json)
        {
            var obj = JsonConvert.DeserializeObject<CarryJUL>(json);
            return new JsonableUniversalLog(){Id = obj.Id,Code = obj.Code,Content = obj.Content,DateCreated = obj.DateCreated,LogType = obj.LogType };
        }

        public static JsonableUniversalLog CreateFrom<T>(SLog<T> log) where T:class
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
