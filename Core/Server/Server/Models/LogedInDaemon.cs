namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.logedInDaemons")]
    public partial class LogedInDaemon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idDaemon { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime expires { get; set; }

        [Required]
        public Guid sessionUuid { get; set; }

        public virtual Daemon daemon { get; set; }
    }
}
