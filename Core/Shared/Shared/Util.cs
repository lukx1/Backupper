﻿using Shared.LogObjects;
using Shared.NetMessages;
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

        public static string GetAppdataFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Backupper");
        }

        public static string GetSharedFolder()
        {
            Directory.CreateDirectory("C:/Backupper");
            return "C:/Backupper";
        }

        public static string GetFileInAppData(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Backupper",fileName);
        }

        public static Exception GetBottomException(Exception e)
        {
            if (e.InnerException == null)
                return e;
            return GetBottomException(e);
        }

    }
}
