namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.DaemonPreSharedKeys")]
    public partial class DaemonPreSharedKey
    {
        public int Id { get; set; }

        public int IdUser { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(68)]
        public string PreSharedKey { get; set; }

        public DateTime Expires { get; set; }

        public bool Used { get; set; }

        public virtual User User { get; set; }
    }
}
