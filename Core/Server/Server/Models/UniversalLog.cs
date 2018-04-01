namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.UniversalLogs")]
    public partial class UniversalLog
    {
        public int Id { get; set; }

        public int IdLogType { get; set; }

        public Guid Code { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Content { get; set; }
    }
}
