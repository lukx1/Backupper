namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.TaskLocations")]
    public partial class TaskLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskLocation()
        {
            TaskLocationLogs = new HashSet<TaskLocationLog>();
            TaskLocationsTimes = new HashSet<TaskLocationsTime>();
            TaskLocationsTimes1 = new HashSet<TaskLocationsTime>();
        }

        public int Id { get; set; }

        public int? IdTask { get; set; }

        public int? IdSource { get; set; }

        public int? IdDestination { get; set; }

        public int? IdBackupTypes { get; set; }

        public virtual BackupType BackupType { get; set; }

        public virtual BackupType BackupType1 { get; set; }

        public virtual Location Location { get; set; }

        public virtual Location Location1 { get; set; }

        public virtual Location Location2 { get; set; }

        public virtual Location Location3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocationLog> TaskLocationLogs { get; set; }

        public virtual Task Task { get; set; }

        public virtual Task Task1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocationsTime> TaskLocationsTimes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocationsTime> TaskLocationsTimes1 { get; set; }
    }
}
