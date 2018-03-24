namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.TaskTimes")]
    public partial class TaskTime
    {
        public int Id { get; set; }

        public int IdTask { get; set; }

        public int IdTime { get; set; }

        public virtual Task Task { get; set; }

        public virtual Time Time { get; set; }
    }
}
