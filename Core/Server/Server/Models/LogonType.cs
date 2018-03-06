namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.LogonTypes")]
    public partial class LogonType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Name { get; set; }
    }
}
