namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.locations")]
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            taskLocations = new HashSet<TaskLocation>();
            taskLocations1 = new HashSet<TaskLocation>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1024)]
        public string uri { get; set; }

        public int idProtocol { get; set; }

        public int idLocationCredentails { get; set; }

        public virtual LocationCredential locationCredential { get; set; }

        public virtual Protocol protocol { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> taskLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> taskLocations1 { get; set; }
    }
}
