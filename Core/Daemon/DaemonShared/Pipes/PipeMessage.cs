using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonShared.Pipes
{
    public class PipeMessage
    {
        public const string PIPE_NAME = "BackupperDeskToServCom";
        public PipeCode Code;
        /// <summary>
        /// ASCII string
        /// </summary>
        public string Payload= "";
        public const int MAX_SIZE_IN_BYTES = 1024; 

        /// <summary>
        /// Serializes any object and sets payload to it
        /// </summary>
        public object SerializePayload
        {
            set => Payload = JsonConvert.SerializeObject(value);
        }

        public T DeserializePayload<T>()
        {
            return JsonConvert.DeserializeObject<T>(Payload);
        }

        public static PipeMessage Read(byte[] message)
        {
            PipeMessage pip = new PipeMessage();
            var msg = StringCompressor.DecompressBytes(message);
            pip.Code = (PipeCode)((msg[0] << 24) + (msg[1] << 16) + (msg[2] << 8) + (msg[3] << 0));
            var bP = msg.Skip(4).ToList();
            bP.RemoveAll(b => b == 4);
            pip.Payload = Encoding.UTF8.GetString(bP.ToArray());
            return pip;
        }

        public byte[] ToSendable()
        {
            MemoryStream stream = new MemoryStream();
            int Code = (int)this.Code;
            byte[] bCode = new byte[] { (byte)(Code >> 24), (byte)(Code >> 16), (byte)(Code >> 8), (byte)(Code) };
            byte[] bPayload = Encoding.UTF8.GetBytes(Payload);
            if (bPayload.Length + bCode.Length > MAX_SIZE_IN_BYTES)
                throw new InvalidOperationException($"Příliš velká zpráva ({bPayload.Length + bCode.Length}), max {MAX_SIZE_IN_BYTES}");
            stream.Write(bCode, 0, bCode.Length);
            stream.Write(bPayload, 0, bPayload.Length);
            byte[] compressed = StringCompressor.CompressByte(stream.ToArray());
            stream.SetLength(0);
            byte[] padding = new byte[MAX_SIZE_IN_BYTES - compressed.Length];
            stream.Write(compressed, 0, compressed.Length);
            for (int i = 0; i < padding.Length; i++)
            {
                padding[i] = 4;
            }
            if (padding.Length > 0)
                stream.Write(padding, 0, padding.Length);
            return stream.ToArray();
        }
    }
}
