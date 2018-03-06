using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class ProjectIni
    {
        public static string SourceName = "ServerConfig.ini";
        public static IniDictionary Data;
        private static string _SOURCE = getSource();
        public static string SOURCE
        {
            get => _SOURCE;
            set
            {
                if (Data != null)
                    Data.Reload(value);
                _SOURCE = value;
            }
        }
        
        
        static ProjectIni()
        {
            SOURCE = getSource();
            Data = new IniDictionary(SOURCE);
        }

        private static string getSource()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Backupper\\ServerConfig.ini");

        }

        public class IniDictionary
        {
            private Dictionary<string, string> inner = new Dictionary<string, string>();

            public string this[string key]
            {
                get
                {
                    return inner[key.ToLower()];
                }
                set
                {
                    inner[key] = Change(key, value);
                }
            }

            public void Reload(string file)
            {
                inner.Clear();
                if (!File.Exists(file))
                    return;
                string[] lines = File.ReadAllLines(file);
                foreach (var item in lines)
                {
                    var res = item.Split(':');
                    if (res.Length != 2)
                        throw new FormatException("Ini " + file + " file is not in key:value format");
                    if (inner.ContainsKey(res[0]))
                        throw new FormatException("Ini " + file + " file contains duplicate keys");
                    inner.Add(res[0], res[1]);
                }
            }

            private string Change(string key, string value)
            {
                Remove(key);
                Add(key, value);
                return value;
            }

            public IniDictionary(string source)
            {
                Reload(SOURCE);
            }

            public void Remove(string key)
            {
                key = key.ToLower();
                List<string> lines = new List<string>();
                foreach (var line in File.ReadLines(SOURCE))
                {
                    lines.Add(line);
                }
                //var lines = File.ReadAllLines(SOURCE);
                string[] linesMinus = new string[lines.Count - 1];

                int inc = 0;
                for (int i = 0; i < lines.Count; i++)
                {
                    string[] res = lines[i].Split(':');
                    if (res[0] == key)
                    {
                        inc++;
                        continue;
                    }
                    else
                        linesMinus[i - inc] = lines[i];
                }
                File.WriteAllLines(SOURCE, linesMinus);
            }

            void InnerAdd(string key, string value)
            {
                Data.InnerAdd(key, value);
            }

            public void Add(string key, string value)
            {

                key = key.ToLower();
                string[] lines = File.ReadAllLines(SOURCE);
                string[] linesPlus = new string[lines.Length + 1];
                lines.CopyTo(linesPlus, 0);
                linesPlus[linesPlus.Length - 1] = key + ":" + value;
                File.WriteAllLines(SOURCE, linesPlus);
            }

        }


    }
}
