namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.locationCredentials")]
    public partial class LocationCredential
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocationCredential()
        {
            locations = new HashSet<Location>();
        }

        public int id { get; set; }

        [StringLength(256)]
        public string host { get; set; }

        public int? port { get; set; }

        public int idLogonType { get; set; }

        [StringLength(128)]
        public string username { get; set; }

        [Column(TypeName = "char")]
        [StringLength(72)]
        public string password { get; set; }

        public virtual LogonType logonType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> locations { get; set; }
    }
}
