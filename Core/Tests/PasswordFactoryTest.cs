using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using System.Security.Cryptography;
using Shared.NetMessages;
using Newtonsoft.Json;

namespace Tests
{
    [TestClass]
    public class PasswordFactoryTest
    {
        [TestMethod]
        public void ComparePasswordsTest()
        {
            string hashed = Shared.PasswordFactory.HashPasswordPbkdf2("asda");
            Assert.IsTrue(Shared.PasswordFactory.ComparePasswordsPbkdf2("asda", hashed));
        }

        [TestMethod]
        public void HashPasswordAES()
        {
            string encrypted = PasswordFactory.EncryptAES("message", "123");
            string result = PasswordFactory.DecryptAES(encrypted, "123");
            Assert.IsTrue(result == "message");
        }

        /*private async void killMe()
        {
            Messenger client = new Messenger("");
            await client.SendPost(new IntroductionMessage()
            {
                macAdress = new char[12] { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                os = "Win10",
                version = 1,
                preSharedKey = "tRnhF0IfmkDrIZU6dbCusQ=="
            );
        }*/

        [TestMethod]
        public void Test()
        {
            /*LoginResponse login = new LoginResponse() { sessionUuid = new Guid(), errorMessage = new StandardResponseMessage() {message = "err" } };
            var json = JsonConvert.SerializeObject(login);
            LoginResponse res = JsonConvert.DeserializeObject<LoginResponse>(json);*/
            Console.WriteLine();
            //NetMessageParse messageParse = new NetMessageParse();
            //string s = JsonConvert.SerializeObject(new PingMessage() { startTime = 100, endTime = 900});
            //INetMessage receive = HttpAdvancedClient.ParseMessage("0{\"startTime\":100,\"endTime\":900}");
            //INetMessage ping = new PingMessage();
            //var res = JsonConvert.DeserializeAnonymousType<PingMessage>("{\"startTime\":100,\"endTime\":900}", (PingMessage)ping);
            // messageParse.parsePingMessage("");
            //killMe();
            Console.WriteLine();
            //jsonResponse.Works = true;

        }

        [TestMethod]
        public void HashMD5()
        {
            uint res0 = PasswordFactory.CalculateCRC32("testAAA");
            uint res1 = PasswordFactory.CalculateCRC32("a");
            uint res2 = PasswordFactory.CalculateCRC32("testAAAssssssssssssssssssssssssssssssssssssssss");
            
        }

    }
}

