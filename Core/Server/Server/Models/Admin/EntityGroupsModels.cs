using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class DaemonGroupsModel
    {
        public int IdDaemon { get; set; }
        public IList<SimpleGroupInfoModel> Groups { get; set; }

        public DaemonGroupsModel()
        {
            Groups = new List<SimpleGroupInfoModel>();
        }

        public DaemonGroupsModel(int id)
        {
            IdDaemon = id;
        }

        public void Load()
        {
            using (var db = new MySQLContext())
            {
                if (db.Daemons.FirstOrDefault(x => x.Id == IdDaemon) == null)
                    throw new Exception("Daemon does not exists");

                var daemonGrp = db.DaemonGroups.Where(x => x.IdDaemon == IdDaemon).ToArray();
                var groups = db.Groups.ToArray();

                Groups = new List<SimpleGroupInfoModel>();

                foreach (var item in groups)
                {
                    Groups.Add(new SimpleGroupInfoModel(item.Id, item.Name, daemonGrp.FirstOrDefault(x => x.IdGroup == item.Id) != null));
                }
            }
        }

        public void Save()
        {
            using (var db = new MySQLContext())
            {
                if (db.Daemons.FirstOrDefault(x => x.Id == IdDaemon) == null)
                    throw new Exception("Daemon does not exists");

                db.DaemonGroups.RemoveRange(db.DaemonGroups.Where(x => x.IdDaemon == IdDaemon));

                var desiredForDb = Groups
                    .Where(x => x.EntityBelongsIn)
                    .Select(x => new DaemonGroup() { IdGroup = x.IdGroup, IdDaemon = IdDaemon });

                db.DaemonGroups.AddRange(desiredForDb);

                db.SaveChanges();
            }
        }
    }

    public class UserGroupsModel
    {
        public int IdUser { get; set; }
        public IList<SimpleGroupInfoModel> Groups { get; set; }

        public UserGroupsModel()
        {
            Groups = new List<SimpleGroupInfoModel>();
        }

        public UserGroupsModel(int id)
        {
            IdUser = id;
        }

        public void Load()
        {
            using (var db = new MySQLContext())
            {
                if(db.Users.FirstOrDefault(x => x.Id == IdUser) == null)
                    throw new Exception("User does not exists");

                var userGrp = db.UserGroups.Where(x => x.IdUser == IdUser).ToArray();
                var groups = db.Groups.ToArray();

                Groups = new List<SimpleGroupInfoModel>();

                foreach (var item in groups)
                {
                    Groups.Add(new SimpleGroupInfoModel(item.Id, item.Name, userGrp.FirstOrDefault(x => x.IdGroup == item.Id) != null));
                }
            }
        }

        public void Save()
        {
            using (var db = new MySQLContext())
            {
                if (db.Users.FirstOrDefault(x => x.Id == IdUser) == null)
                    throw new Exception("User does not exists");

                db.UserGroups.RemoveRange(db.UserGroups.Where(x => x.IdUser == IdUser));

                var desiredForDb = Groups.Where(x => x.EntityBelongsIn)
                    .Select(x => new UserGroup() {IdGroup = x.IdGroup, IdUser = IdUser});

                db.UserGroups.AddRange(desiredForDb);

                db.SaveChanges();
            }
        }
    }

    public class SimpleGroupInfoModel
    {
        public int IdGroup { get; set; }
        public string GroupName { get; set; }
        public bool EntityBelongsIn { get; set; }

        public SimpleGroupInfoModel(int idGroup, string groupName, bool entityBelongsIn)
        {
            IdGroup = idGroup;
            GroupName = groupName;
            EntityBelongsIn = entityBelongsIn;
        }

        public SimpleGroupInfoModel()
        {
        }
    }
}