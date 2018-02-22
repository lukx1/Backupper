using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using Shared.NetMessages;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tests
{
    /// <summary>
    /// Summary description for HttpAdvancedClientTest
    /// </summary>
    [TestClass]
    public class MessengerTest
    {
        [TestCategory("HTTPSend")]
        [TestMethod]
        public void Send()
        {
            try
            {
                Messenger messenger = new Messenger(@"http://localhost:57597");
                messenger.Send(new IntroductionMessage(), "introduction", HttpMethod.Post);
                var response = messenger.ReadMessage<IntroductionResponse>();
                Console.WriteLine();
            }
            catch(AggregateException e)
            {
                if (e.InnerException != null && e.InnerException is HttpRequestException)
                    Assert.Inconclusive("Nešlo se připojit k serveru");
                else
                    throw e;
            }
        }

        private async Task RealSendBulkAsync()
        {

            Messenger messenger = new Messenger(@"http://localhost:57597");
            List<Task<string>> tasks = new List<Task<string>>();
            for (int i = 0; i < 3; i++)
            {
                tasks.Add(messenger.SendAsyncGetJson(new IntroductionMessage(), "introduction", HttpMethod.Post));
            }
            string[] responses = await Task.WhenAll(tasks);
            foreach (var item in responses)
            {
                Assert.IsTrue(item != null, "Nepřišla odpověď");
            }
        }

        private async Task<IntroductionResponse> RealSendAsync()
        {
            Messenger messenger = new Messenger(@"http://localhost:57597");
            var json = await messenger.SendAsyncGetJson(new IntroductionMessage(), "introduction", HttpMethod.Post);
            return messenger.ReadMessage<IntroductionResponse>(json);
        }

        [TestCategory("HTTPSend")]
        [TestMethod]
        public void SendAsync()
        {
            try
            {
                var task = RealSendAsync();
                task.Wait();
                IntroductionResponse response = task.Result;
                Assert.IsNotNull(response);
            }
            catch (AggregateException e)
            {
                if (e.InnerException != null && e.InnerException is HttpRequestException)
                    Assert.Inconclusive("Nešlo se připojit k serveru");
                else
                    throw e;
            }
        }
        [TestCategory("HTTPSend")]
        [TestMethod]
        public void SendBulkAsync()
        {
            try
            {
                RealSendBulkAsync().Wait();
            }
            catch (AggregateException e)
            {
                if (e.InnerException != null && e.InnerException is HttpRequestException)
                    Assert.Inconclusive("Nešlo se připojit k serveru");
                else
                    throw e;
            }
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
