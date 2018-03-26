using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class GroupPermissionsModel
    {
        public GroupPermissionsModel()
        {
            Permissions = new List<SimplePermissionModel>();
        }

        public GroupPermissionsModel(int id)
        {
            IdGroup = id;
        }

        public int IdGroup { get; set; }
        public IList<SimplePermissionModel> Permissions { get; set; }

        public void Load()
        {
            using (var db = new MySQLContext())
            {
                if (db.Groups.FirstOrDefault(x => x.Id == IdGroup) == null)
                    throw new Exception("Group does not exists");

                var groupPermissions = db.GroupPermissions.Where(x => x.IdGroup == IdGroup).ToArray();
                var permissions = db.Permissions.ToArray();

                Permissions = new List<SimplePermissionModel>();

                foreach (var item in permissions)
                {
                    Permissions.Add(new SimplePermissionModel(item.Id, item.Name, groupPermissions.FirstOrDefault(x => x.IdPermission == item.Id) != null));
                }
            }
        }

        public void Save()
        {
            using (var db = new MySQLContext())
            {
                if (db.Groups.FirstOrDefault(x => x.Id == IdGroup) == null)
                    throw new Exception("Group does not exists");

                db.GroupPermissions.RemoveRange(db.GroupPermissions.Where(x => x.IdGroup == IdGroup));

                var desiredForDb = Permissions
                    .Where(x => x.GroupHasIt)
                    .Select(x => new GroupPermission() { IdPermission = x.IdPermission, IdGroup = IdGroup });

                db.GroupPermissions.AddRange(desiredForDb);

                db.SaveChanges();
            }
        }
    }

    public class SimplePermissionModel
    {
        public SimplePermissionModel()
        {
        }

        public SimplePermissionModel(int id, string name, bool groupHasIt)
        {
            IdPermission = id;
            Name = name;
            GroupHasIt = groupHasIt;
        }

        public int IdPermission { get; set; }
        public string Name { get; set; }
        public bool GroupHasIt { get; set; }
    }
}