namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.Tasks")]
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            TaskLocations = new HashSet<TaskLocation>();
            TaskTimes = new HashSet<TaskTime>();
        }

        public int Id { get; set; }

        public int IdDaemon { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int IdTaskDetails { get; set; }

        public int IdBackupTypes { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime LastChanged { get; set; }

        [StringLength(4096)]
        public string ActionBefore { get; set; }

        [StringLength(4096)]
        public string ActionAfter { get; set; }

        public virtual BackupType BackupType { get; set; }

        public virtual Daemon Daemon { get; set; }

        public virtual TaskDetail TaskDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocation> TaskLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskTime> TaskTimes { get; set; }
    }
}
