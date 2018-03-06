namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.LogedInDaemons")]
    public partial class LogedInDaemon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdDaemon { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime Expires { get; set; }

        public Guid SessionUuid { get; set; }

        public virtual Daemon Daemon { get; set; }
    }
}
