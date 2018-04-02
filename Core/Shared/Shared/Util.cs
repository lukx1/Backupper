using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Užitečné nástroje které nikam jinam nepatří
    /// </summary>
    public static class Util
    {
        public const bool IsDebug = false;
        public const string Newline = "\r\n"; //TODO dodelat
        public static string GetFileInAppData(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Backupper\\"+fileName);
        }
        public static Exception GetBottomException(Exception e)
        {
            if (e.InnerException == null)
                return e;
            return GetBottomException(e);
        }
    }
}
