namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.UpTimes")]
    public partial class UpTime
    {
        public int Id { get; set; }

        public int IdSource { get; set; }

        public bool IsDaemon { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
