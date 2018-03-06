namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.TaskLocationsTimes")]
    public partial class TaskLocationsTime
    {
        public int Id { get; set; }

        public int? IdTaskLocation { get; set; }

        public int? IdTime { get; set; }

        public virtual TaskLocation TaskLocation { get; set; }

        public virtual Time Time { get; set; }
    }
}
