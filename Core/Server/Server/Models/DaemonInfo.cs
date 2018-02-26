namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.DaemonInfos")]
    public partial class DaemonInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DaemonInfo()
        {
            Daemons = new HashSet<Daemon>();
            Daemons1 = new HashSet<Daemon>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Os { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(12)]
        public string Mac { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DateAdded { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Daemon> Daemons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Daemon> Daemons1 { get; set; }
    }
}
