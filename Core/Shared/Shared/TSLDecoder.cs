using Shared.ToDaemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TSLDecoder
    {
        public int ByteCount;
        public TDMessageCode Code;

        private static int ReadByteSize(NetworkStream stream)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4)
                throw new InvalidOperationException("Nebylo prijato dost bytu");
            return BitConverter.ToInt32(buffer, 0);
        }

        private static TDMessageCode GetCode(NetworkStream stream)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4)
                throw new InvalidOperationException("Nebylo prijato dost bytu");
            return (TDMessageCode)BitConverter.ToInt32(buffer, 0);
        }

        public byte[] Decode(NetworkStream stream)
        {
            ByteCount = ReadByteSize(stream);
            Code = GetCode(stream);
            byte[] buff = new byte[ByteCount];
            stream.Read(buff, 0, ByteCount);

            return buff;
        }
    }
}
