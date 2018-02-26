using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.NetMessages.TaskMessages;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Tests
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void TaskCreate()
        {
            DbTask task = new DbTask()
            {
                name = "DebugTask",
                description = "For debugging",
                taskLocations = new System.Collections.Generic.List<TaskLocation>()
                {
                    new TaskLocation()
                    {
                        backupType = BackupType.NORM,
                        times = new System.Collections.Generic.List<Time>()
                        {
                            new Time(){interval=0,name="Dneska",startTime = DateTime.Now.AddHours(1),repeat = false},
                            new Time(){interval=24*3600*7,name="Kazdy Patek",startTime = DateTime.Parse("2018-02-23"),repeat = true},
                        },
                        destination = new Location()
                        {
                            protocol = Protocol.WND,
                            uri = @"C:\Users\myName\Desktop\Docs**",
                            LocationCredential = new LocationCredential()
                            {
                                host = "test.com/myName",password="abc",port=21,username="myName",LogonType = LogonType.Normal
                            }
                        },
                        source = new Location()
                        {
                            protocol = Protocol.FTP,
                            uri = "test.com",
                            LocationCredential = new LocationCredential()
                            {
                                host = "test.com/myName",password="abc",port=21,username="myName",LogonType = LogonType.Normal
                            }
                        }
                    }
                }
            };
            Console.WriteLine(JsonConvert.SerializeObject(task));
        }
    }
}
