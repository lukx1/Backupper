namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.WaitingForOneClick")]
    public partial class WaitingForOneClick
    {
        public int Id { get; set; }

        public int IdDaemonInfo { get; set; }

        [Required]
        [StringLength(100)]
        public string User { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DateReceived { get; set; }

        public bool Confirmed { get; set; }

        public virtual DaemonInfo DaemonInfo { get; set; }
    }
}
