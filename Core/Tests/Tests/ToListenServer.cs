﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using Shared.NetMessages.FromDaemon;
using System.Text;
using Shared;

namespace Tests
{
    [TestClass]
    public class ToListenServer
    {
        /*[TestMethod]
        public void TcpClient()
        {
            TcpClient client = new TcpClient(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"),7556));
            var stream = client.GetStream();
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new TaskUpdateMessage()));
            stream.Write(bytes, 0, bytes.Length);
            TSLDecoder decoder = new TSLDecoder();
            var msg = decoder.Decode(stream);
            Console.WriteLine();
        }*/
    }
}
