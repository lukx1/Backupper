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

        public int Id { get; } = -1;
        public LogType LogType { get => LogType.DEBUG; }
        public LogContentType Code { get => LogContentType.DEBUG; }
        public DateTime DateCreated => DateTime.Now;
        public ILogHeader Header => MyHeader; // Proc nemuzu mit rovnou SimpleHeader kdyz SimpleHeader implementuje ILogHeader
        public DebugLogContents Content => new DebugLogContents();

        public SimpleHeader MyHeader = new SimpleHeader();

        public class DebugLogContents
        {
            /// <summary>
            /// Je nutno následovat pravidla pro serializaci a deserializaci jsonu!
            /// </summary>
            public bool Debugging { get; set; } = true;
        }
    }
}
