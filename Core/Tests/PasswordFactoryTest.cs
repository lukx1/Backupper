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
            string hashed = Shared.PasswordFactory.HashPassword("asda");
            Assert.IsTrue(Shared.PasswordFactory.ComparePasswords("asda", hashed));
        }
    }
}

