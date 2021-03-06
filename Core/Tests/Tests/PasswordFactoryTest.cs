﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using System.Security.Cryptography;
using Shared.NetMessages;


namespace Tests
{
    [TestClass]
    public class PasswordFactoryTest
    {
        [TestMethod]
        public void ComparePbkdf2()
        {
            string hashed = Shared.PasswordFactory.HashPasswordPbkdf2("asda");
            Assert.IsTrue(hashed.Length == 68, "Delka Pbkdf neni 68 charu");
            Assert.IsTrue(Shared.PasswordFactory.ComparePasswordsPbkdf2("asda", hashed), "Sifra nerozlustena");
        }

        [TestMethod]
        public void HashPasswordAES()
        {
            string encrypted = PasswordFactory.EncryptAES("message", "123");
            string result = PasswordFactory.DecryptAES(encrypted, "123");
            Assert.IsTrue(encrypted.Length == 72,"Delka AES neni 72 charu");
            Assert.IsTrue(result == "message","Sifra nerozlustena");
        }

        //[TestMethod]
        //public void Test()
        //{
        //    var pred = DateTime.Compare(DateTime.Parse("2000-01-02"), DateTime.Parse("2000-01-03"));
        //    var v = DateTime.Compare(DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01"));
        //    var po = DateTime.Compare(DateTime.Parse("2000-01-02"), DateTime.Parse("2000-01-01"));
        //    /*LoginResponse login = new LoginResponse() { sessionUuid = new Guid(), errorMessage = new StandardResponseMessage() {message = "err" } };
        //    var json = JsonConvert.SerializeObject(login);
        //    LoginResponse res = JsonConvert.DeserializeObject<LoginResponse>(json);*/
        //    Console.WriteLine();
        //    //NetMessageParse messageParse = new NetMessageParse();
        //    //string s = JsonConvert.SerializeObject(new PingMessage() { startTime = 100, endTime = 900});
        //    //INetMessage receive = HttpAdvancedClient.ParseMessage("0{\"startTime\":100,\"endTime\":900}");
        //    //INetMessage ping = new PingMessage();
        //    //var res = JsonConvert.DeserializeAnonymousType<PingMessage>("{\"startTime\":100,\"endTime\":900}", (PingMessage)ping);
        //    // messageParse.parsePingMessage("");
        //    //killMe();
        //    Console.WriteLine();
        //    //jsonResponse.Works = true;
        //    Assert.Inconclusive("Pouze na testovani");

        //}

        [TestMethod]
        public void CRC32()
        {
            uint a = PasswordFactory.CalculateCRC32("adqwudghdsivbsiafbsdlkihafnbadsf");
            uint b = PasswordFactory.CalculateCRC32("adqwudghdsivbsiafbsdlkihafnbadsf");
            uint c = PasswordFactory.CalculateCRC32("casdwdfhxfopohwqiodhjfhaslfgewhi");
            Assert.IsTrue(a == b,"CRC32 dvou stejnych objektu neni stejne");
            Assert.IsTrue(a != c,"CRC32 dvou rozdilnych objektu je stejne");
        }

    }
}

