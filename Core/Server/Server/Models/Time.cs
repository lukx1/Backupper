namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.times")]
    public partial class Time
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Time()
        {
            taskLocationsTimes = new HashSet<TaskLocationsTime>();
        }

        public int id { get; set; }

        public int? interval { get; set; }

        [Required]
        [StringLength(40)]
        public string name { get; set; }

        public bool repeat { get; set; }

        public DateTime startTime { get; set; }

        public DateTime? endTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskLocationsTime> taskLocationsTimes { get; set; }
    }
}
