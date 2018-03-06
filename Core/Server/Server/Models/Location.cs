namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.Locations")]
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            TaskLocations = new HashSet<TaskLocation>();
            TaskLocations1 = new HashSet<TaskLocation>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(1024)]
        public string Uri { get; set; }

        public int? IdProtocol { get; set; }

        public int? IdLocationCredentails { get; set; }

        public virtual LocationCredential LocationCredential { get; set; }

        public virtual Protocol Protocol { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> TaskLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> TaskLocations1 { get; set; }
    }
}
