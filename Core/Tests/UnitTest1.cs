using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}

