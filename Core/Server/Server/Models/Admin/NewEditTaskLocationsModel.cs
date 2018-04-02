using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class NewEditTaskLocationsModel
    {
        public int? Id { get; set; }
        public int IdTask { get; set; }
        public int IdSource { get; set; }
        public int IdDestination { get; set; }

        public IList<Location> Locations { get; set; } = new List<Location>();

        public void Load()
        {
            using (var db = new MySQLContext())
            {
                Locations = db.Locations.ToList();

                if (Id.HasValue)
                {
                    var taskLoc = db.TaskLocations.FirstOrDefault(x => x.Id == Id.Value);
                    if (taskLoc == null)
                        throw new Exception("Task location does not exists");
                }
            }
        }

        public void Save()
        {
            using (var db = new MySQLContext())
            {
                if (Id.HasValue)
                {
                    db.TaskLocations.AddOrUpdate(new TaskLocation()
                    {
                        Id = Id.Value,
                        IdTask = IdTask,
                        IdSource = IdSource,
                        IdDestination = IdDestination
                    });
                }
                else
                {
                    db.TaskLocations.Add(new TaskLocation()
                    {
                        IdTask = IdTask,
                        IdSource = IdSource,
                        IdDestination = IdDestination
                    });
                }

                db.SaveChanges();
            }
        }
    }
}