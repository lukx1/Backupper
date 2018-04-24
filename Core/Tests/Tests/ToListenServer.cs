using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using Shared.NetMessages.FromDaemon;
using System.Text;
using Shared;
using Shared.ToDaemon;

namespace Tests
{
    [TestClass]
    public class ToListenServer
    {
        [TestMethod]
        public void TcpClient()
        {
            //TcpClient client = new TcpClient(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"),7556));
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7556));
            var stream = new NetworkStream(client);
            var bytes = TDMessage.FromObject(new TDMessage.Message() { Code = TDMessageCode.TaskUpdate, SerializeToContent = new TaskUpdateMessage() { Update = true} }).ContentsAsArray;
            stream.Write(bytes, 0, bytes.Length);
            TSLDecoder decoder = new TSLDecoder();
            var msg = decoder.Decode(stream);
            Console.WriteLine();
        }
    }
}
