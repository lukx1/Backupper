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
    public class MessengerTest
    {
        [TestMethod]
        public void Login()
        {
            Messenger messenger = new Messenger(@"http://localhost:57597");
            messenger.SendPost(new IntroductionMessage(), "introduction");
            var response = messenger.ReadMessage<IntroductionResponse>();
            Console.WriteLine();
        }

        /*private async void adasd(Messenger client)
        {
            var res = await client.SendPost(new PingMessage());
            Console.WriteLine();
        }

        [TestMethod]
        public void HttpTest()
        {
            Messenger client = new Messenger();
            adasd(client);
        }*/
    }
}
