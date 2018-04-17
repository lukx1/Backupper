using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class ShowOrAssignCredentialModel
    {
        public int IdLocation { get; set; }
        public int? IdCredential { get; set; }
        public IList<LocationCredential> Credentials { get; set; }

        public ShowOrAssignCredentialModel()
        {

        }

        public ShowOrAssignCredentialModel(int idLocation)
        {
            IdLocation = idLocation;

            using (var db = new MySQLContext())
            {
                var loc = db.Locations.FirstOrDefault(x => x.Id == IdLocation);
                if (loc == null)
                    throw new Exception("Location does not exists");

                IdCredential = loc.IdLocationCredentails;

                Credentials = db.LocationCredentials.AsQueryable().Include(x => x.LogonType).ToArray();
            }
        }

        public void Save()
        {
            using (var db = new MySQLContext())
            {
                var loc = db.Locations.FirstOrDefault(x => x.Id == IdLocation);
                if (loc == null)
                    throw new Exception("Location does not exists");

                loc.IdLocationCredentails = IdCredential;
                db.Entry(loc).State = EntityState.Modified;

                foreach (var sourceLoc in loc.TaskLocations)
                {
                    sourceLoc.Task.LastChanged = DateTime.Now;
                    db.Entry(sourceLoc.Task).State = EntityState.Modified;
                }

                foreach (var destLoc in loc.TaskLocations)
                {
                    destLoc.Task.LastChanged = DateTime.Now;
                    db.Entry(destLoc.Task).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}