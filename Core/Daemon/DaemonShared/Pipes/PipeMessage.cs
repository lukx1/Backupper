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
        public string Payload;
        public const int MAX_SIZE_IN_BYTES = 1024; 

        public static PipeMessage Read(byte[] msg, byte[] code = null)
        {
            PipeMessage pip = new PipeMessage();
            if(code == null)
                pip.Code = (PipeCode)((msg[0] << 24) + (msg[1] << 16) + (msg[2] << 8) + (msg[3] << 0));
            var bP = msg.Skip(4).ToList();
            bP.RemoveAll(b => b == 4);
            pip.Payload = Encoding.ASCII.GetString(bP.ToArray());
            return pip;
        }

        public byte[] ToSendable()
        {
            MemoryStream stream = new MemoryStream();
            int Code = (int)this.Code;
            byte[] bCode = new byte[]{ (byte)(Code >> 24), (byte)(Code >> 16), (byte)(Code >> 8), (byte)(Code) };
            stream.Write(bCode, 0, 4);
            byte[] bPayload = Encoding.ASCII.GetBytes(Payload ?? "");
            if(bPayload.Length+bCode.Length > MAX_SIZE_IN_BYTES)
                throw new InvalidOperationException($"Příliš velká zpráva ({bPayload.Length + bCode.Length}), max {MAX_SIZE_IN_BYTES}");
            stream.Write(bPayload, 0, bPayload.Length);
            byte[] padding = new byte[MAX_SIZE_IN_BYTES - (bCode.Length + bPayload.Length)];
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
