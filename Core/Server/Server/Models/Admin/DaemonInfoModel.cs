using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class DaemonInfoModel
    {
        public static IEnumerable<DaemonInfoModel> GetAllOnlyBasicInfo()
        {
            List<DaemonInfoModel> result = new List<DaemonInfoModel>();

            using (MySQLContext db = new MySQLContext())
            {
                var daemons = db.Daemons.ToList();
                if (daemons.Count == 0)
                    throw new Exception("No daemons");

                foreach (var item in daemons)
                {
                    result.Add(new DaemonInfoModel() { Id = item.Id, IdUser = item.IdUser, DaemonInfo = item.DaemonInfo, Nickname = item.User.Nickname });
                }
            }

            return result;
        }

        public void Populate()
        {
            using (MySQLContext db = new MySQLContext())
            {
                var daemon = db.Daemons.FirstOrDefault(x => x.Id == Id);
                if (daemon == null)
                    throw new Exception("Daemon does not exists");

                IdUser = daemon.IdUser;
                Nickname = daemon.User.Nickname;
                DaemonInfo = daemon.DaemonInfo;
                Tasks = daemon.Tasks.ToArray();
            }
        }

        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Nickname { get; set; }
        public DaemonInfo DaemonInfo { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}