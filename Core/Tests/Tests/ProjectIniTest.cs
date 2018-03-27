using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System.IO;

namespace Tests
{
    /// <summary>
    /// Summary description for ProjectIniTest
    /// </summary>
    /*[TestClass]
    public class ProjectIniTest
    {

        private string SOURCE;

        [TestInitialize]
        public void TestInitialize()
        {
            SOURCE = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("Core") + 4) + @"\Server\Server\ServerConfig.ini";
            ProjectIni.SOURCE = SOURCE;
        }


        [TestMethod]
        public void ReadTest()
        {
            var name = ProjectIni.Data["sqlname"];
            Assert.IsTrue(name != null);
        }

        [TestMethod]
        public void WriteTest()
        { 
            var name = ProjectIni.Data["sqlname"];
            Assert.IsTrue(ProjectIni.Data["sqlname"] == name);
            ProjectIni.Data["sqlname"] = "testName";
            Assert.IsTrue(ProjectIni.Data["sqlname"] == "testName");
            ProjectIni.Data["sqlname"] = name;
            Assert.IsTrue(ProjectIni.Data["sqlname"] == name);
        }

    }*/
}
