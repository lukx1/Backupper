namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.DaemonGroups")]
    public partial class DaemonGroup
    {
        public int Id { get; set; }

        public int? IdDaemon { get; set; }

        public int? IdGroup { get; set; }

        public virtual Daemon Daemon { get; set; }

        public virtual Group Group { get; set; }
    }
}
