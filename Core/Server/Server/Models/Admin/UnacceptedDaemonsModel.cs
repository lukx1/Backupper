using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.Admin
{
    public class UnacceptedDaemonsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDaemonAccepted { get; set; }
    }
}