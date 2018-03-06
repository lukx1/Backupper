namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.DaemonLogs")]
    public partial class DaemonLog
    {
        public int Id { get; set; }

        public int IdDaemon { get; set; }

        public int IdLogType { get; set; }

        public int Code { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(64)]
        public string ShortText { get; set; }

        [Required]
        [StringLength(512)]
        public string LongText { get; set; }

        public virtual LogType LogType { get; set; }

        public virtual User User { get; set; }
    }
}
