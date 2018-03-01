﻿using Server.Authentication;
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
        private Authenticator authenticator = new Authenticator();
        private MySQLContext mysql;
        public List<ErrorMessage> errors = new List<ErrorMessage>();

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

        private DbTask ExtractData(Task task)
        {
                var dbTask = new DbTask() { name = task.Name, description = task.Description,id=task.Id,uuidDaemon=task.Daemon.Uuid };
                List<Shared.NetMessages.TaskMessages.TaskLocation> taskLocations = new List<Shared.NetMessages.TaskMessages.TaskLocation>();
                dbTask.taskLocations = taskLocations;
                foreach (var taskLocation in mysql.TaskLocations.Where(r => r.IdTask == task.Id))
                {

                    Shared.NetMessages.TaskMessages.TaskLocation dbTaskLocation = new Shared.NetMessages.TaskMessages.TaskLocation();
                    taskLocations.Add(dbTaskLocation);
                    dbTaskLocation.id = taskLocation.Id;
                    dbTaskLocation.backupType = new Shared.NetMessages.TaskMessages.BackupType()
                    {
                        Id = taskLocation.BackupType.Id,
                        LongName = taskLocation.BackupType.LongName,
                        ShortName = taskLocation.BackupType.ShortName
                    };
                    dbTaskLocation.source = new Shared.NetMessages.TaskMessages.Location()
                    {
                        id = taskLocation.Location.Id,
                        uri = taskLocation.Location.Uri,
                        protocol = new Shared.NetMessages.TaskMessages.Protocol()
                        {
                            Id = taskLocation.Location.Protocol.Id,
                            LongName = taskLocation.Location.Protocol.LongName,
                            ShortName = taskLocation.Location.Protocol.ShortName
                        },
                        LocationCredential = new Shared.NetMessages.TaskMessages.LocationCredential()
                        {
                            Id = taskLocation.Location.LocationCredential.Id,
                            host = taskLocation.Location.LocationCredential.Host,
                            password = taskLocation.Location.LocationCredential.Password,
                            port = (int)taskLocation.Location.LocationCredential.Port,
                            username = taskLocation.Location.LocationCredential.Username,
                            LogonType = new Shared.NetMessages.TaskMessages.LogonType()
                            {
                                Id = taskLocation.Location.LocationCredential.LogonType.Id,
                                Name = taskLocation.Location.LocationCredential.LogonType.Name
                            }
                        }
                    };
                dbTaskLocation.destination = new Shared.NetMessages.TaskMessages.Location()
                {
                    id = taskLocation.Location1.Id,
                    uri = taskLocation.Location1.Uri,
                    protocol = new Shared.NetMessages.TaskMessages.Protocol()
                    {
                        Id = taskLocation.Location1.Protocol.Id,
                        LongName = taskLocation.Location1.Protocol.LongName,
                        ShortName = taskLocation.Location1.Protocol.ShortName
                    },
                    LocationCredential = new Shared.NetMessages.TaskMessages.LocationCredential()
                    {
                        Id = taskLocation.Location1.LocationCredential.Id,
                        host = taskLocation.Location1.LocationCredential.Host,
                        password = taskLocation.Location1.LocationCredential.Password,
                        port = (int)taskLocation.Location1.LocationCredential.Port,
                        username = taskLocation.Location1.LocationCredential.Username,
                        LogonType = new Shared.NetMessages.TaskMessages.LogonType()
                        {
                            Id = taskLocation.Location1.LocationCredential.LogonType.Id,
                            Name = taskLocation.Location1.LocationCredential.LogonType.Name
                        }
                    }
                };
                List<Shared.NetMessages.TaskMessages.Time> times = new List<Shared.NetMessages.TaskMessages.Time>();
                    foreach (var taskLocationTime in mysql.TaskLocationsTimes.Where(r => r.IdTaskLocation == taskLocation.Id))
                    {
                        times.Add(new Shared.NetMessages.TaskMessages.Time()
                        {
                            id = taskLocationTime.Time.Id,
                            endTime = taskLocationTime.Time.EndTime,
                            interval = taskLocationTime.Time.Interval,
                            name = taskLocationTime.Time.Name,
                            repeat = taskLocationTime.Time.Repeat,
                            startTime = taskLocationTime.Time.StartTime
                        });
                    }
                    dbTaskLocation.times = times;
                }
                return dbTask;
            }

        private List<DbTask> FetchAll(TaskMessage message)
        {
            int idDaemon = authenticator.GetDaemonFromUuid(message.sessionUuid).Id;
            var tasks = mysql.Tasks.Where(r => r.IdDaemon == idDaemon);
            List<DbTask> dbTasks = new List<DbTask>();
            int i = 0;
            foreach (var task in tasks)
            {
                try
                {
                    dbTasks.Add(ExtractData(task));
                }
                catch(Exception e)
                {
                    errors.Add(new ErrorMessage() {id=e.HResult,message=e.Message,value=i.ToString() });
                }
                finally
                {
                    i++;
                }
            }
            return dbTasks;
        }

        public List<DbTask> GetTasks(TaskMessage message)
        {
            if (message.tasks == null | message.tasks.Count == 0)
                return FetchAll(message);
            else
            {
                List<DbTask> tasks = new List<DbTask>();
                for (int i = 0; i < message.tasks.Count; i++)
                {
                    tasks.Add(ExtractData(mysql.Tasks.Where(r => r.Id == message.tasks[i].id).FirstOrDefault()));
                }
                return tasks;
            }
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
                catch(Exception e)
                {
                    result[i] = new ErrorMessage() { id = 2, message = "Task #"+i+" selhal\r\n"+e.Message, value = i.ToString() };
                }
            }
            return result;
        }
    }
}