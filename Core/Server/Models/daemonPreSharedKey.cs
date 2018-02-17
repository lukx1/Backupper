namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.daemonPreSharedKeys")]
    public partial class DaemonPreSharedKey
    {
        public int id { get; set; }

        public int idUser { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(68)]
        public string preSharedKey { get; set; }

        public DateTime expires { get; set; }

        public bool used { get; set; }
    }
}
