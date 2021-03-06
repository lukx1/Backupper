﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Server.Objects.AdminExceptions;

namespace Server.Models.Admin
{
    public class LocationModel
    {
        public Location DbLocation { get; set; }
        public IEnumerable<SelectListItem> Protocols { get; set; }

        public LocationModel()
        {
            DbLocation = new Location();
            LoadProtocols();
        }

        public LocationModel(int idLocation)
        {
            using (var db = new MySQLContext())
            {
                DbLocation = db.Locations.AsQueryable().Where(x => x.Id == idLocation).Include(x => x.Protocol).FirstOrDefault();
                if(DbLocation == null)
                    throw new AdminException("Location not found");
            }

            LoadProtocols();

            var tmp = Protocols.FirstOrDefault(x => x.Value == DbLocation.IdProtocol.ToString());
            if (tmp != null)
                tmp.Selected = true;
        }

        private void LoadProtocols()
        {
            using (var db = new MySQLContext())
            {
                Protocols = new SelectList(db.Protocols.ToArray(), "Id", "ShortName");

                Protocols = db.Protocols.Select(x => new SelectListItem { Text = x.ShortName + " - " + x.LongName, Value = x.Id.ToString() }).ToArray();
            }
        }

        public void Create()
        {
            using (var db = new MySQLContext())
            {
                var loc = db.Locations.Add(DbLocation);
                
                db.SaveChanges();
            }
        }

        public void Update()
        {
            using (var db = new MySQLContext())
            {
                var loc = db.Locations.FirstOrDefault(x => x.Id == DbLocation.Id);
                loc.IdLocationCredentails = DbLocation.IdLocationCredentails;
                loc.IdProtocol = DbLocation.IdProtocol;
                loc.Uri = DbLocation.Uri;
                db.Entry(loc).State = EntityState.Modified;

                foreach (var sourceLoc in loc.TaskLocations)
                {
                    sourceLoc.Task.LastChanged = DateTime.Now;
                    db.Entry(sourceLoc.Task).State = EntityState.Modified;
                }

                foreach (var destLoc in loc.TaskLocations1)
                {
                    destLoc.Task.LastChanged = DateTime.Now;
                    db.Entry(destLoc.Task).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }

        public void Delete()
        {
            using (var db = new MySQLContext())
            {
                var loc = db.Locations.Remove(DbLocation);

                foreach (var sourceLoc in loc.TaskLocations)
                {
                    sourceLoc.Task.LastChanged = DateTime.Now;
                    db.Entry(sourceLoc.Task).State = EntityState.Modified;
                }

                foreach (var destLoc in loc.TaskLocations1)
                {
                    destLoc.Task.LastChanged = DateTime.Now;
                    db.Entry(destLoc.Task).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}