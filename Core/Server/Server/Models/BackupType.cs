namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.BackupTypes")]
    public partial class BackupType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BackupType()
        {
            TaskLocations = new HashSet<TaskLocation>();
        }

        public int Id { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(4)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(32)]
        public string LongName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> TaskLocations { get; set; }
    }
}
