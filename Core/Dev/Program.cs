using Shared;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        static void Main(string[] args)
        {
            //CreateParsingScripts();
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:57597/");
            //client.PostAsync("/api/login",new StringContent("\"{\"uuid\":\"50a7cd9f - d5f9 - 4c40 - 8e0f - bfcbb21a5f0e\",\"password\":\"VO0e + 84BW4wqVYsuUpGeWw == \"}\"")));
            Messenger messenger = new Messenger("http://localhost:57597/");
            messenger.SendPost(new LoginMessage(),"login");
            var response = messenger.ReadMessage<LoginMessage>();
            Console.WriteLine(response.ToString());
            //PresharedMaker();
            Console.ReadLine();
        }
    }
}
