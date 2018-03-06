namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.LocationCredentials")]
    public partial class LocationCredential
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocationCredential()
        {
            Locations = new HashSet<Location>();
        }

        public int Id { get; set; }

        [StringLength(256)]
        public string Host { get; set; }

        public int? Port { get; set; }

        public int? IdLogonType { get; set; }

        [StringLength(128)]
        public string Username { get; set; }

        [Column(TypeName = "char")]
        [StringLength(72)]
        public string Password { get; set; }

        public virtual LogType LogType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> Locations { get; set; }
    }
}
