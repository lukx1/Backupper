using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    /// <summary>
    /// Vzorová třída pro využití ILog a SpecificLog
    /// </summary>
    public class DebugLog : ILog<DebugLog.DebugLogContents>
    {

        public int Id { get; private set; } = -1;
        public LogType LogType { get => LogType.DEBUG; }
        public LogContentType Code { get => LogContentType.DEBUG; }
        public DateTime DateCreated { get; private set; }
        public DebugLogContents Content { get => _Content; private set => _Content = value; }

        private DebugLogContents _Content=  new DebugLogContents();

        public class DebugLogContents
        {
            /// <summary>
            /// Je nutno následovat pravidla pro serializaci a deserializaci jsonu!
            /// </summary>
            public bool Debugging { get; set; } = true;
        }

        public void Load(JsonableUniversalLog universalLog)
        {
            this.Id = universalLog.Id;
            this.DateCreated = universalLog.DateCreated;
            this.Content = JsonConvert.DeserializeObject<DebugLogContents>(universalLog.Content);
        }
    }
}
