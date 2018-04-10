using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Pokud Hází chybu "Type T must be a reference type" tak musíte dopsat where T : class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SLog <T> where T : class
    {
        public int Id { get; set; } = -1;
        public LogType LogType { get; set; } = LogType.INFORMATION;
        public abstract LogContentType Code { get; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public abstract T Content { get; protected set; }

        protected T ParseContent(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public void Load(JsonableUniversalLog universalLog)
        {
            if(this.Code.Uuid != universalLog.Code)
            {
                throw new ArgumentException("Kód universal logu a tohoto logu se neshoduje");
            }
            this.Content = ParseContent(universalLog.Content);
            this.Id = universalLog.Id;
            this.LogType = universalLog.LogType;
            this.DateCreated = universalLog.DateCreated;
        }

    }
}
