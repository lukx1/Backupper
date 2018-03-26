using Server.Authentication;
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

        private Models.Location CreateLocationFromLocation(Shared.NetMessages.TaskMessages.DbLocation location)
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

        [Obsolete]
        private Models.Task CreateTaskFromTask(Shared.NetMessages.TaskMessages.DbTask task, Daemon daemon)
        {
            /*var res =  new Task()
            {
                Daemon = daemon,
                Name = task.name,
                Description = task.description,
            };
            foreach (var time in task.times)
            {
                TaskTime taskTime = new TaskTime();
                taskTime.Task = res;
                taskTime.Time = new Time()
                {
                    EndTime = time.endTime,
                    Interval = time.interval,
                    Name = time.name,
                    Repeat = time.repeat,
                    StartTime = time.startTime
                };
                res.TaskTimes.Add(taskTime);
            }*/
            throw new NotImplementedException();//Tato funkce by nikdy nemela byt volana
        }

        private Models.TaskLocation CreateTaskLocationFromTaskLocation(Shared.NetMessages.TaskMessages.DbTaskLocation taskLocation, Models.Task task)
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
                Location = source,
                Location1 = destination,
            };
        }

        private Models.Time CreateTimeFromTime(Shared.NetMessages.TaskMessages.DbTime time)
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
            }
            mysql.SaveChanges();
        }

        private DbLocation CreateLocation(TaskLocation taskLocation, bool source)
        {
            var location = source ? taskLocation.Location : taskLocation.Location1;

            var protocol = new Shared.NetMessages.TaskMessages.DbProtocol()
            {
                Id = location.Protocol.Id,
                LongName = location.Protocol.LongName,
                ShortName = location.Protocol.ShortName
            };
            var locCred = new Shared.NetMessages.TaskMessages.DbLocationCredential()
            {
                Id = location.LocationCredential.Id,
                host = location.LocationCredential.Host,
                password = location.LocationCredential.Password,
                port = location.LocationCredential.Port == null ? 0 : (int)location.LocationCredential.Port,
                username = location.LocationCredential.Username,

            };
            if (location.LocationCredential != null)
            {
                locCred.LogonType =
             new Shared.NetMessages.TaskMessages.DbLogonType()
             {
                 Id = location.LocationCredential.LogonType.Id,
                 Name = location.LocationCredential.LogonType.Name
             };
            }

            var dbLoc = new Shared.NetMessages.TaskMessages.DbLocation()
            {
                id = location.Id,
                uri = location.Uri,
            };
            dbLoc.protocol = protocol;
            dbLoc.LocationCredential = locCred;
            return dbLoc;
        }

        private DbTask ExtractData(Task task)
        {
            var dbTask = new DbTask()
            {
                name = task.Name,
                backupType = new DbBackupType()
                {
                    Id = task.BackupType.Id,
                    LongName = task.BackupType.LongName,
                    ShortName = task.BackupType.ShortName
                },
                details = new DbTaskDetails()
                {
                    Id = task.TaskDetail.Id,
                    CompressionLevel =task.TaskDetail.CompressionLevel,
                    ZipAlgorithm = task.TaskDetail.ZipAlgorithm
                },
                description = task.Description,
                id = task.Id, uuidDaemon = task.Daemon.Uuid,
                lastChanged = task.LastChanged,
            };

            //Nastavi casy
            foreach (var taskTimes in task.TaskTimes)
            {
                dbTask.times.Add(new Shared.NetMessages.TaskMessages.DbTime()
                {
                    id = taskTimes.Time.Id,
                    endTime = taskTimes.Time.EndTime,
                    interval = taskTimes.Time.Interval,
                    name = taskTimes.Time.Name,
                    repeat = taskTimes.Time.Repeat,
                    startTime = taskTimes.Time.StartTime
                });
            }

            List<Shared.NetMessages.TaskMessages.DbTaskLocation> taskLocations = new List<Shared.NetMessages.TaskMessages.DbTaskLocation>();
            dbTask.taskLocations = taskLocations;
            foreach (var taskLocation in mysql.TaskLocations.Where(r => r.IdTask == task.Id))
            {

                Shared.NetMessages.TaskMessages.DbTaskLocation dbTaskLocation = new Shared.NetMessages.TaskMessages.DbTaskLocation();
                taskLocations.Add(dbTaskLocation);
                //Nastavení tasklocationu
                dbTaskLocation.id = taskLocation.Id;
                //Přidání lokací
                dbTaskLocation.source = CreateLocation(taskLocation, true);
                dbTaskLocation.destination = CreateLocation(taskLocation, false);

                //Tasklocationdetails
                

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
                catch (Exception e)
                {
                    errors.Add(new ErrorMessage() { id = e.HResult, message = e.Message, value = i.ToString() });
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
                throw new NotImplementedException("Tato část není dokončena");
                List<DbTask> tasks = new List<DbTask>(); 
                for (int i = 0; i < message.tasks.Count; i++)
                {
                    tasks.Add(ExtractData(mysql.Tasks.Where(r => r.Id == message.tasks[i].id).FirstOrDefault())); //TODO: Fixnout tenhle fetch
                }
                return tasks;
            }
        }

        public ErrorMessage[] CreateTasks(TaskMessage message)
        {
            var logedInDaemons = mysql.LogedInDaemons.Where(r => r.SessionUuid == message.sessionUuid).FirstOrDefault();
            var daemon = mysql.Daemons.Where(r => r.Id == logedInDaemons.IdDaemon).FirstOrDefault();

            if (daemon == null)
                return new ErrorMessage[1] { new ErrorMessage() { id = 3, message = "Neplatný daemon" } };
            if (message.tasks == null)
                return new ErrorMessage[1] { new ErrorMessage() { id = 1, message = "Přijatý prázdný list" } };
            ErrorMessage[] result = new ErrorMessage[message.tasks.Count];
            for (int i = 0; i < message.tasks.Count; i++)
            {
                DbTask task = message.tasks[i];
                try
                {
                    CreateTask(task, daemon);
                }
                catch (Exception e)
                {
                    result[i] = new ErrorMessage() { id = 2, message = "Task #" + i + " selhal\r\n" + e.Message, value = i.ToString() };
                }
            }
            return result;
        }
    }
}