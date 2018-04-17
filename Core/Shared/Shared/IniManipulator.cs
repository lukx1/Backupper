using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class IniManipulator
    {
        public string filePath { get; private set; }
        private const char delimiter = '=';
        public string defaultResp = null;

        private struct KeyVal
        {
            public string key;
            public string val;

            public KeyVal(string[] kv)
            {
                if (kv == null || kv.Length != 2)
                    throw new ArgumentException("kv");
                this.key = kv[0];
                this.val = kv[1];
            }

            public KeyVal(string key, string val)
            {
                this.key = key.ToLower();
                this.val = val;
            }

        }

        public IniManipulator(string filePath)
        {
            this.filePath = filePath;
            if (!File.Exists(filePath))
                File.Create(filePath);
        }

        public bool Remove(string key)
        {
            key = key.ToLower();
            StringBuilder builder = new StringBuilder();
            bool removed = false;
            foreach (var line in File.ReadLines(filePath))
            {
                KeyVal kv = new KeyVal(line.Split(delimiter));
                if (kv.key == key)
                {
                    removed = true;
                    continue;
                }
                else
                    builder.Append(kv.key).Append(delimiter).Append(kv.val);
                builder.Append(Environment.NewLine);
            }
            File.WriteAllText(filePath, builder.ToString());
            return removed;
        }

        public void Write(string key, string val)
        {
            key = key.ToLower();
            StringBuilder builder = new StringBuilder();
            bool written = false;
            foreach (var line in File.ReadLines(filePath))
            {
                KeyVal kv = new KeyVal(line.Split(delimiter));
                if (kv.key == key)
                {
                    builder.Append(key).Append(delimiter).Append(val);
                    written = true;
                }
                else
                    builder.Append(kv.key).Append(delimiter).Append(kv.val);
                builder.Append(Environment.NewLine);
            }
            if(!written)
                builder.Append(key).Append(delimiter).Append(val).Append(Environment.NewLine);
            File.WriteAllText(filePath, builder.ToString());
        }
        public string Read(string key)
        {
            key = key.ToLower();
            foreach (var line in File.ReadLines(filePath))
            {
                KeyVal kv = new KeyVal(line.Split(delimiter));
                if (kv.key == key)
                    return kv.val;
            }
            return defaultResp;
        }
    }
}
