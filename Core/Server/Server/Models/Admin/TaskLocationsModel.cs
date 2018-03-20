using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class TaskLocationsModel
    {
        public TaskLocationsModel(int id) => IdTask = id;

        public void Populate()
        {
            using (MySQLContext db = new MySQLContext())
            {
                var task = db.Tasks.FirstOrDefault(x => x.Id == IdTask);
                if (task == null)
                    throw new Exception("Task does not exists");

                IdDaemon = task.IdDaemon;
                TaskLocations = task.TaskLocations.ToArray();
            }
        }

        public int IdTask { get; set; }
        public int? IdDaemon { get; set; }
        public IEnumerable<TaskLocation> TaskLocations { get; set; }
    }
}