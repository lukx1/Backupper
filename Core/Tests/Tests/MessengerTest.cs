using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using Shared.NetMessages;
using System.Net.Sockets;
using System.Threading.Tasks;
using Shared.NetMessages.TaskMessages;
using Newtonsoft.Json;

namespace Tests
{
    /// <summary>
    /// Summary description for HttpAdvancedClientTest
    /// </summary>
    [TestClass]
    public class MessengerTest
    {
        /*[TestCategory("HTTPSend")]
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

        [TestCategory("Controller")]
        [TestMethod]
        public void TaskTest()
        {
            DbTask task = new DbTask()
            {
                name = "DebugTask",
                description = "For debugging",
                taskLocations = new System.Collections.Generic.List<TaskLocation>()
                {
                    new TaskLocation()
                    {
                        backupType = BackupType.NORM,
                        times = new System.Collections.Generic.List<Time>()
                        {
                            new Time(){interval=0,name="Dneska",startTime = DateTime.Now.AddHours(1),repeat = false},
                            new Time(){interval=24*3600*7,name="Kazdy Patek",startTime = DateTime.Parse("2018-02-23"),repeat = true},
                        },
                        destination = new Location()
                        {
                            protocol = Protocol.WND,
                            uri = @"C:\Users\myName\Desktop\Docs**",
                            LocationCredential = new LocationCredential()
                            {
                                host = "test.com/myName",password="abc",port=21,username="myName",LogonType = LogonType.Normal
                            }
                        },
                        source = new Location()
                        {
                            protocol = Protocol.FTP,
                            uri = "test.com",
                            LocationCredential = new LocationCredential()
                            {
                                host = "test.com/myName",password="abc",port=21,username="myName",LogonType = LogonType.Normal
                            }
                        }
                    }
                }
            };
            Messenger messenger = new Messenger(@"http://localhost:57597");
            messenger.Send(new LoginMessage() { uuid = new Guid("50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e"), password = "VO0e+84BW4wqVYsuUpGeWw==" }, "login", HttpMethod.Post);
            var login = messenger.ReadMessage<LoginResponse>();
            var taskMessage = new TaskMessage() { sessionUuid = login.sessionUuid, tasks = new List<DbTask>() { task } };
            var jsonTaskMessage = JsonConvert.SerializeObject(taskMessage);
            messenger.Send(taskMessage, "task", HttpMethod.Post);
            var res = messenger.ReadMessage<TaskResponse>();
            Console.WriteLine();
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
