using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaemonShared.Pipes;
using System.IO.Compression;

namespace DaemonShared.Pipes
{
    public class PipeMessage
    {
        public const string PIPE_NAME = "BackupperDeskToServCom";
        public PipeCode Code;
        /// <summary>
        /// UTF8 string
        /// </summary>
        public string Payload= "";
        public const int MAX_SIZE_IN_BYTES = 4096; 

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

    internal static class StringCompressor
    {
        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            return Convert.ToBase64String(CompressByte(Encoding.UTF8.GetBytes(text)));
        }

        public static byte[] CompressByte(byte[] bytes)
        {
            byte[] buffer = bytes;
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return gZipBuffer;
        }

        public static byte[] DecompressBytes(byte[] bytes)
        {
            byte[] gZipBuffer = bytes;
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return buffer;
            }
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            return Encoding.UTF8.GetString(DecompressBytes(Encoding.UTF8.GetBytes(compressedText)));
        }
    }
}
