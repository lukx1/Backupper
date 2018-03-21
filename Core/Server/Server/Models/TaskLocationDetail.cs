namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.TaskLocationDetails")]
    public partial class TaskLocationDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskLocationDetail()
        {
            TaskLocations = new HashSet<TaskLocation>();
        }

        public int Id { get; set; }

        [StringLength(32)]
        public string ZipAlgorithm { get; set; }

        public int? CompressionLevel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> TaskLocations { get; set; }
    }
}
