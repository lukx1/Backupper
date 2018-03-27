namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.Daemons")]
    public partial class Daemon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Daemon()
        {
            DaemonGroups = new HashSet<DaemonGroup>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }

        public Guid Uuid { get; set; }

        public int IdUser { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(68)]
        public string Password { get; set; }

        public int IdDaemonInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DaemonGroup> DaemonGroups { get; set; }

        public virtual DaemonInfo DaemonInfo { get; set; }

        public virtual User User { get; set; }

        public virtual LogedInDaemon LogedInDaemon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
