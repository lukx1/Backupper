namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.TaskLocations")]
    public partial class TaskLocation
    {
        public int Id { get; set; }

        public int IdTask { get; set; }

        public int IdSource { get; set; }

        public int IdDestination { get; set; }

        public virtual Location Location { get; set; }

        public virtual Location Location1 { get; set; }

        public virtual Task Task { get; set; }
    }
}
