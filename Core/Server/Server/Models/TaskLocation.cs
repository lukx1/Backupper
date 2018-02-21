namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.taskLocations")]
    public partial class TaskLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskLocation()
        {
            taskLocationsTimes = new HashSet<TaskLocationsTime>();
        }

        public int id { get; set; }

        public int idTask { get; set; }

        public int idSource { get; set; }

        public int idDestination { get; set; }

        public int idBackupTypes { get; set; }

        public virtual BackupType backupType { get; set; }

        public virtual Location location { get; set; }

        public virtual Location location1 { get; set; }

        public virtual Task task { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocationsTime> taskLocationsTimes { get; set; }
    }
}
