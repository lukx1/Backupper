using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogObjects
{
    /// <summary>
    /// Vzorová třída pro využití SLog a SpecificLog
    /// </summary>
    public class DebugLog : SLog<DebugLog.DebugLogContents>
    {
        public override LogContentType Code => LogContentType.DEBUG;

        public override DebugLogContents Content { get; protected set; } = new DebugLogContents();

        public class DebugLogContents
        {
            /// <summary>
            /// Je nutno následovat pravidla pro serializaci a deserializaci jsonu!
            /// </summary>
            public bool Debugging { get; set; } = true;
        }

    }
}
