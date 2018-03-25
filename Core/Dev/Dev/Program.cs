using Newtonsoft.Json;
using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dev
{
    class Program
    {
        private static void CreateParsingScripts()
        {
            List<string> filePaths = Directory.GetFiles(@"..\..\..\Shared\NetMessages\", "*.cs",
                                         SearchOption.TopDirectoryOnly).ToList<string>();
            foreach (var item in filePaths)
            {
                string name = item.Substring(item.LastIndexOf('\\')+1).Replace(".cs","");
                if (!Regex.IsMatch(name,"^[a-zA-Z0-9]+Message$") || name == "INetMessage")
                    continue;
                string n = Type.GetType("Dev.Program").FullName;
                INetMessage netMessage = (INetMessage)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance("Shared.NetMessages."+name);
                Console.WriteLine(name);
            }
            Console.WriteLine();
            /*var res =  NetMessageHelper.GetAll();
            foreach (var item in res.Values)
            {
                string name = item.GetType().Name;
                string exec = "public static "+name+" Parse"+name.Replace("Message","")+"(string json)\n" +
                "{\n" +
                    "\tvar mt = ParseMessage(json);\n" +
                    "\treturn JsonConvert.DeserializeAnonymousType(mt.msg, (PingMessage)mt.tmp);\n" +
                "}\n";
                Console.WriteLine(exec);
            }*/
        }

        private static void PresharedMaker()
        {
            for (int i = 0; i < 10; i++)
            {
                var unhashed = PasswordFactory.CreateRandomPassword(16);
                var hashed = PasswordFactory.HashPasswordPbkdf2(unhashed);
                Console.WriteLine("unhashed : "+unhashed+"\nhashed : "+hashed+"\n");
            }
            
        }

        private async static void ad()
        {
            /*HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57597/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var result = await client.PostAsync("api/login", new StringContent("\"{\"uuid\":\"50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e\",\"password\":\"VO0e+84BW4wqVYsuUpGeWw==\"}\"", Encoding.UTF8, "application/json"));

            var content = new StringContent("\"{\"uuid\":\"50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e\",\"password\":\"VO0e+84BW4wqVYsuUpGeWw==\"}\"", Encoding.UTF8, "application/json");
            //content.Headers.Add("Content-Type", "application/json");
            var result = await client.PostAsync("api/login", content);
            
            Console.WriteLine((int)result.StatusCode);*/
            new Messenger("");
            /*HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57597/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/login");
            request.Content = new StringContent("sadasfdasdfadfadfsd",
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            client.SendAsync(request)
                  .ContinueWith(responseTask =>
                  {
                      Console.WriteLine("Response: {0}", responseTask.Result);
                  });*/
        }

        /*private static async void Test()
        {
            Messenger messenger = new Messenger("http://localhost:57597/");
            await messenger.SendAsync(new LoginMessage(), "login", HttpMethod.Post).ContinueWith(r => Console.WriteLine(messenger.ReadMessage<LoginResponse>()));
            Console.ReadLine();
            //Console.WriteLine();
        }*/

        private static void DoHash(params string[] passes)
        {
            foreach (var item in passes)
            {
                var hash = PasswordFactory.HashPasswordPbkdf2(item);
                var isSame = PasswordFactory.ComparePasswordsPbkdf2(item, hash);
                Console.WriteLine(hash+"\r\n"+isSame);
            }
            
            Console.ReadLine();
        }

        public class B {
            public string c;
        }

        private void Test() { 
}

        public static void Main(string[] args)
        {
            ////DoHash("Admin123456");
            //Messenger messenger = new Messenger(@"http://localhost:57597");
            //messenger.Send(new LoginMessage() { uuid = new Guid("50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e"), password = "VO0e+84BW4wqVYsuUpGeWw==" }, "login", HttpMethod.Post);
            //var login = messenger.ReadMessage<LoginResponse>();
            //var taskMessage = new TaskMessage() { sessionUuid = login.sessionUuid };
            //messenger.Send(taskMessage, "task", HttpMethod.Post);
            //var res = messenger.ReadMessage<TaskResponse>();
            Console.WriteLine();
            Console.ReadLine();
        }

        /*static void Main(string[] args)
        {
            Task theTask = ProcessAsync();
            int x = 0;  // set breakpoint
            theTask.Wait();
        }

        static async Task ProcessAsync()
        {
            var result = await DoSomethingAsync();  // set breakpoint

            int y = 0;  // set breakpoint
        }

        static async Task<int> DoSomethingAsync()
        {
            await Task.Delay(1000);
            return 5;
        }*/

        /*static void Main(string[] args)
        {
            //CreateParsingScripts();

            //ad();
            Test();
            
            Messenger messenger = new Messenger("http://localhost:57597/");
            messenger.SendPost(new LoginMessage(),"login");
            var response = messenger.ReadMessage<LoginMessage>();
            Console.WriteLine(response.ToString());
            //PresharedMaker();
            //Console.ReadLine();
        }*/
    }
}
