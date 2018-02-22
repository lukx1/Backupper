using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Shared
{
    public class PcInfo : IDisposable
    {

        private string file;
        private List<string> data;
        private string[] sysInfoGet = new string[]
        {
           "MachineId","SystemManufacturer","SystemModel","Processor","Memory"
        };
        private string[] displayDeviceGet = new string[]
        {
           "CardName","Manufacturer","ChipType","DeviceType"
        };

        private bool ShouldGet(string nodeName, string[] list)
        {
            return list.Contains(nodeName);
        }

        /// <summary>
        /// Vytvoří dxdiag soubor, freezuje než je vytvořen
        /// </summary>
        public void CreateDxDiag()
        {
            file = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".diag";
            System.Diagnostics.Process.Start("dxdiag.exe", "/x " + file);
            while (!File.Exists(file)){
                Thread.Sleep(25);
            }
        }

        /// <summary>
        /// Získá UID jako 24 charový string
        /// </summary>
        /// <returns></returns>
        public string GetUniqueId()
        {
            return GetUniqueId(16);
        }

        /// <summary>
        /// Získá UID
        /// </summary>
        /// <returns></returns>
        public string GetUniqueId(int bytes)
        {
            byte[] result = new byte[bytes];
            for (int i = 0; i < data.Count; i++)
            {
                byte[] rand = Encoding.UTF8.GetBytes(data[i]);
                for (int y = 0; y < result.Length; y++)
                {
                    byte b = rand[y % rand.Length];
                    result[y] = b;
                    result[y] ^= (byte)(y << 2);
                    result[y] ^= (byte)(y >> 6);
                    result[y] ^= (byte)(y << 3);
                }

            }
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Přeloží získáný soubor
        /// </summary>
        public void ParseDxDiag()
        {
            ParseDxDiag(file);
        }

        /// <summary>
        /// Přeloží jakýkoliv dxdiag soubor
        /// </summary>
        /// <param name="file"></param>
        public void ParseDxDiag(string file)
        {
            data = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode sysInfo = doc.DocumentElement.SelectSingleNode("/DxDiag/SystemInformation");
            foreach (XmlNode node in sysInfo.ChildNodes)
            {
                if (ShouldGet(node.Name, sysInfoGet))
                    data.Add(node.InnerText);
            }
            XmlNode displayInfo = doc.DocumentElement.SelectSingleNode("/DxDiag/DisplayDevices/DisplayDevice");
            foreach (XmlNode node in displayInfo.ChildNodes)
            {
                if (ShouldGet(node.Name, displayDeviceGet))
                    data.Add(node.InnerText);
            }
        }

        public void Dispose()
        {
            data = null;
        }
    }
}
