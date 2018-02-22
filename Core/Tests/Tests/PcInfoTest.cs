using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;

namespace Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PcInfoTest
    {
        [TestMethod]
        public void PcInfo()
        {
            PcInfo pcInfo = new PcInfo();
            //pcInfo.CreateDxDiag();
            pcInfo.ParseDxDiag(@"C:\Users\lukx\AppData\Local\Temp\dbe15557-73ac-4e92-87a1-f08b3bfeaf40.diag");
            var uid = pcInfo.GetUniqueId();
            Console.WriteLine();
        }
    }
}
