namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.backupTypes")]
    public partial class BackupType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BackupType()
        {
            taskLocations = new HashSet<TaskLocation>();
        }

        public int id { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(4)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(32)]
        public string LongName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> taskLocations { get; set; }
    }
}
