namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.taskLocationsTimes")]
    public partial class TaskLocationsTime
    {
        public int id { get; set; }

        public int idTaskLocation { get; set; }

        public int idTime { get; set; }

        public virtual TaskLocation taskLocation { get; set; }

        public virtual Time time { get; set; }
    }
}
