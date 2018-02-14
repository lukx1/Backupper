using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using System.Security.Cryptography;

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

    }
}

