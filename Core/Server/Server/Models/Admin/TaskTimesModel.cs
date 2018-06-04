using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class TaskTimesModel
    {
        public TaskTimesModel()
        {
            TaskTimes = new List<SimpleTaskTimeModel>();
        }

        public TaskTimesModel(int id)
        {
            IdTask = id;
            TaskTimes = new List<SimpleTaskTimeModel>();
        }

        public int IdTask { get; set; }
        public int IdDaemon { get; set; }
        public IList<SimpleTaskTimeModel> TaskTimes { get; set; }

        public void Load()
        {
            using (var db = new MySQLContext())
            {
                var task = db.Tasks.FirstOrDefault(x => x.Id == IdTask);
                if (task == null)
                    throw new Exception("Task does not exists");

                IdDaemon = task.IdDaemon;

                var taskTimes = task.TaskTimes.ToArray();
                var times = db.Times.ToArray();

                foreach (var item in times)
                {
                    TaskTimes.Add(new SimpleTaskTimeModel(item.Id, item.Name, taskTimes.FirstOrDefault(x => x.IdTime == item.Id) != null));   
                }
            }
        }

        public void Save()
        {
            using (var db = new MySQLContext())
            {
                var task = db.Tasks.FirstOrDefault(x => x.Id == IdTask);
                if (task == null)
                    throw new Exception("Task does not exists");

                db.TaskTimes.RemoveRange(db.TaskTimes.Where(x => x.IdTask == IdTask));

                var desire = TaskTimes
                    .Where(x => x.IsUsed)
                    .Select(x => new TaskTime() {IdTask = IdTask, IdTime = x.IdTime});

                db.TaskTimes.AddRange(desire);

                task.LastChanged = DateTime.Now;

                db.SaveChanges();
            }
        }
    }

    public class SimpleTaskTimeModel
    {
        public SimpleTaskTimeModel() { }

        public SimpleTaskTimeModel(int id, string name, bool isUsed)
        {
            IdTime = id;
            Name = name;
            IsUsed = isUsed;
        }

        public int IdTime { get; set; }
        public string Name { get; set; }
        public bool IsUsed { get; set; }
    }
}