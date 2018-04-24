using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class IniReader
    {
        private string filePath;

        public IniReader(string filePath)
        {
            this.filePath = filePath;
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
        }

        private struct KeyVal
        {
            public string Key;
            public string Value;
            public KeyVal(string[] arr)
            {
                if (arr.Length != 2)
                    throw new ArgumentException("arr.Length != 2");
                Key = arr[0];
                Value = arr[1];
            }
        }


        public string Read(string key)
        {
            key = key.ToLower();
            foreach (var line in File.ReadAllLines(filePath))
            {
                KeyVal keyVal = new KeyVal(line.Split(':'));
                if (keyVal.Key == key)
                    return keyVal.Value;
            }
            return null;
        }

        public void Write(string key, string value)
        {
            key = key.ToLower();
            StringBuilder builder = new StringBuilder();
            bool foundMatch = false;
            foreach (var line in File.ReadAllLines(filePath))
            {
                KeyVal keyVal = new KeyVal(line.Split(':'));
                if (keyVal.Key == key)
                {
                    builder.Append(key).Append(':').Append(value);
                    foundMatch = true;
                }
                else
                    builder.Append(keyVal.Key).Append(':').Append(keyVal.Value);
                builder.Append(Environment.NewLine);
            }
            if(!foundMatch)
                builder.Append(key).Append(':').Append(value).Append(Environment.NewLine);
            File.WriteAllText(filePath, builder.ToString());
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

    }
}
