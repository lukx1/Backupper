using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using Shared.NetMessages;

namespace Tests
{
    /// <summary>
    /// Summary description for HttpAdvancedClientTest
    /// </summary>
    [TestClass]
    public class HttpAdvancedClientTest
    {

        private async void adasd(HttpAdvancedClient client)
        {
            var res = await client.SendPost(new PingMessage());
            Console.WriteLine();
        }

        [TestMethod]
        public void HttpTest()
        {
            HttpAdvancedClient client = new HttpAdvancedClient();
            adasd(client);
        }
    }
}
