using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Version
    {
        public int Major;
        public int Minor;
        public int Patch;
        public string Extesion = "";

        public static Version Parse(string s)
        {
            string[] parts = s.Split('.');
            if (parts.Length != 3)
                throw new FormatException("Neskládá se ze tří částí oddělených tečkou");

            if(s.Split('-')[0] == s)
            {
                return new Version()
                {
                    Major = int.Parse(parts[0]),
                    Minor = int.Parse(parts[1]),
                    Patch = int.Parse(parts[2])
                };
            }
            else
            {
                if (s.Split('-').Length != 2)
                    throw new FormatException("Je dovolený pouze jeden extension označený pomocí pomlčky");
                return new Version()
                {
                    Major = int.Parse(parts[0]),
                    Minor = int.Parse(parts[1]),
                    Patch = int.Parse(parts[2]),
                    Extesion = s.Split('-')[1]
                };
            }


        }

        public override string ToString()
        {
            if (Extesion.Trim() == "")
                return $"{Major}.{Minor}.{Patch}";
            return $"{Major}.{Minor}.{Patch}-{Extesion}";
        }

    }
}
