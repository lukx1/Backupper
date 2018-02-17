namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.daemons")]
    public partial class Daemon
    {
        public int id { get; set; }

        public Guid uuid { get; set; }

        public int idUser { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(68)]
        public string password { get; set; }

        public int idDaemonInfo { get; set; }

        public virtual DaemonInfo daemonInfo { get; set; }
    }
}
