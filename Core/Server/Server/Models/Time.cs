namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.Times")]
    public partial class Time
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Time()
        {
            TaskLocationsTimes = new HashSet<TaskLocationsTime>();
        }

        public int Id { get; set; }

        public int? Interval { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public bool Repeat { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocationsTime> TaskLocationsTimes { get; set; }
    }
}
