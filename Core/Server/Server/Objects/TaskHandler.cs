using Server.Models;
using Shared.NetMessages;
using Shared.NetMessages.TaskMessages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Server.Objects
{
    public class TaskHandler
    {

        private MySQLContext mysql;

        public TaskHandler()
        {
            this.mysql = new MySQLContext();
        }

        private Models.Location CreateLocationFromLocation(Shared.NetMessages.TaskMessages.Location location)
        {
            Models.LocationCredential locationCredential = new Models.LocationCredential();
            locationCredential.Id = location.LocationCredential.Id;
            locationCredential.IdLogonType = location.LocationCredential.Id;

            return new Models.Location()
            {
                LocationCredential = locationCredential,
                IdProtocol = location.protocol.Id,
                Uri = location.uri
            };
        }

        private Models.Task CreateTaskFromTask(Shared.NetMessages.TaskMessages.DbTask task, Daemon daemon)
        {
            return new Task()
            {
                Daemon = daemon,
                Name = task.name,
                Description = task.description
            };
        }



        private Models.TaskLocation CreateTaskLocationFromTaskLocation(Shared.NetMessages.TaskMessages.TaskLocation taskLocation, Models.Task task)
        {
            var source = new Models.Location()
            {

                
                Uri = taskLocation.source.uri,
                IdProtocol = taskLocation.source.protocol.Id,
                LocationCredential = new Models.LocationCredential()
                {
                    Host = taskLocation.source.LocationCredential.host,
                    Port = taskLocation.source.LocationCredential.port,
                    IdLogonType = taskLocation.source.LocationCredential.LogonType.Id,
                    Username = taskLocation.source.LocationCredential.username,
                    Password = taskLocation.source.LocationCredential.password,
                }
            };
            var destination = new Models.Location()
            {
                Uri = taskLocation.source.uri,
                IdProtocol = taskLocation.source.protocol.Id,
                LocationCredential = new Models.LocationCredential()
                {
                    Host = taskLocation.source.LocationCredential.host,
                    Port = taskLocation.source.LocationCredential.port,
                    IdLogonType = taskLocation.source.LocationCredential.LogonType.Id,
                    Username = taskLocation.source.LocationCredential.username,
                    Password = taskLocation.source.LocationCredential.password,
                }
            };
            return new Models.TaskLocation()
            {
                Task = task,
                Location= source,
                Location1 = destination,
                IdBackupTypes = taskLocation.backupType.Id
            };
        }

        private Models.Time CreateTimeFromTime(Shared.NetMessages.TaskMessages.Time time)
        {
            return new Models.Time()
            {
                EndTime = time.endTime,
                Interval = time.interval,
                Name = time.name,
                Repeat = time.repeat,
                StartTime = time.startTime
            };
        }

        private Models.TaskLocationsTime CreateTaskLocationsTimeFromTaskLocationsTime(Models.TaskLocation taskLocation, Models.Time time)
        {
            return new Models.TaskLocationsTime()
            {
                TaskLocation = taskLocation,
                Time = time
            };
        }

        private void CreateTask(DbTask task, Daemon daemon)
        {
            Models.Task rTask = CreateTaskFromTask(task, daemon);
            mysql.Tasks.Add(rTask);

            foreach (var taskLocation in task.taskLocations)
            {
                Models.TaskLocation rTaskLocation = CreateTaskLocationFromTaskLocation(taskLocation, rTask);
                mysql.TaskLocations.Add(rTaskLocation);

                foreach (var time in taskLocation.times)
                {
                    Models.Time rTime = CreateTimeFromTime(time);
                    mysql.Times.Add(rTime);

                    Models.TaskLocationsTime rTaskLocationsTime = CreateTaskLocationsTimeFromTaskLocationsTime(rTaskLocation, rTime);
                    mysql.TaskLocationsTimes.Add(rTaskLocationsTime);

                }
            }
            mysql.SaveChanges();
        }

        public ErrorMessage[] CreateTasks(TaskMessage message)
        {
            var logedInDaemons = mysql.LogedInDaemons.Where(r => r.SessionUuid == message.sessionUuid).FirstOrDefault();
            var daemon = mysql.Daemons.Where(r => r.Id == logedInDaemons.IdDaemon).FirstOrDefault();

            if(daemon == null)
                return new ErrorMessage[1] { new ErrorMessage() { id = 3, message = "Neplatný daemon" } };
            if (message.tasks == null)
                return new ErrorMessage[1] { new ErrorMessage() { id = 1, message = "Přijatý prázdný list"} };
            ErrorMessage[] result = new ErrorMessage[message.tasks.Count];
            for (int i = 0; i < message.tasks.Count; i++)
            {
                DbTask task = message.tasks[i];
                try
                {
                    CreateTask(task,daemon);
                }
                catch(SqlException e)
                {
                    result[i] = new ErrorMessage() { id = 2, message = e.Message, value = e.ErrorCode.ToString() };
                }
            }
            return result;
        }
    }
}