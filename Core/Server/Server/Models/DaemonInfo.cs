namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.daemonInfos")]
    public partial class DaemonInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DaemonInfo()
        {
            daemons = new HashSet<Daemon>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(64)]
        public string os { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(12)]
        public string mac { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime dateAdded { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Daemon> daemons { get; set; }
    }
}
