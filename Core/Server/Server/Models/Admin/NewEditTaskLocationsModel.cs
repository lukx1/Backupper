using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public IEnumerable<Location> Locations { get; set; }

        public NewEditTaskLocationsModel()
        {

        }

        public void Load()
        {
            using (var db = new MySQLContext())
            {
                Locations = db.Locations.AsQueryable().Include(x => x.Protocol).ToArray();

                if (Id.HasValue)
                {
                    var taskLoc = db.TaskLocations.FirstOrDefault(x => x.Id == Id.Value);
                    if (taskLoc == null)
                        throw new Exception("Task location does not exists");

                    IdTask = taskLoc.IdTask;
                    IdSource = taskLoc.IdSource;
                    IdDestination = taskLoc.IdDestination;
                }
            }
        }

        public void Save()
        {
            using (var db = new MySQLContext())
            {
                if (Id.HasValue)
                {
                    var tLoc = db.TaskLocations.FirstOrDefault(x => x.Id == Id.Value);
                    if (tLoc == null)
                        throw new Exception("Task location does not exists");

                    tLoc.IdDestination = IdDestination;
                    tLoc.IdSource = IdSource;

                    db.Entry(tLoc).State = EntityState.Modified;
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