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
        public static readonly string SOURCE = getSource();
        public static IniDictionary Data = new IniDictionary(SOURCE);

        private static string getSource()
        {
            if (System.Diagnostics.Debugger.IsAttached) {
                return Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("Core")+4)+@"\Server\Server\ServerConfig.ini";
            }
            else
                return @"ServerConfig.ini";
        }

        public class IniDictionary
        {
            private Dictionary<string, string> inner = new Dictionary<string, string>();
            public string this[string key] { get => inner[key.ToLower()]; set => inner[key] = Change(key,value); }

            private string Change(string key, string value)
            {
                Remove(key);
                Add(key,value);
                return value;
            }

            public IniDictionary(string source)
            {
                string[] lines = File.ReadAllLines(source);
                foreach (var item in lines)
                {
                    var res = item.Split(':');
                    if (res.Length != 2)
                        throw new FormatException("Ini "+source+" file is not in key:value format");
                    if (inner.ContainsKey(res[0]))
                        throw new FormatException("Ini " + source + " file contains duplicate keys");
                    inner.Add(res[0],res[1]);
                }
            }

            public void Remove(string key)
            {
                key = key.ToLower();
                var lines = File.ReadAllLines(SOURCE);
                string[] linesMinus = new string[lines.Length - 1];
                Data.Remove(key);
                int inc = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] res = lines[i].Split(':');
                    if (res[0] == key)
                    {
                        inc++;
                        continue;
                    }
                    else
                        linesMinus[i-inc] = lines[i];
                }
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
                lines.CopyTo(linesPlus,0);
                linesPlus[linesPlus.Length - 1] = key + ":" + value;
                File.WriteAllLines(SOURCE,linesPlus);
            }

        }


    }
}
