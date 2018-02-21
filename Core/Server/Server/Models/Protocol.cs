namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.protocols")]
    public partial class Protocol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Protocol()
        {
            locations = new HashSet<Location>();
        }

        public int id { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(3)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(32)]
        public string LongName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> locations { get; set; }
    }
}
