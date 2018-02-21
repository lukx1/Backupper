using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;

namespace Tests
{
    /// <summary>
    /// Summary description for ProjectIniTest
    /// </summary>
    [TestClass]
    public class ProjectIniTest
    {
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

    }
}
