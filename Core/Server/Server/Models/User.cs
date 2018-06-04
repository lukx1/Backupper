namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.Users")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            DaemonPreSharedKeys = new HashSet<DaemonPreSharedKey>();
            Daemons = new HashSet<Daemon>();
            UserGroups = new HashSet<UserGroup>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nickname { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(68)]
        public string Password { get; set; }

        [Required]
        [StringLength(415)]
        public string PublicKey { get; set; }

        [Required]
        [StringLength(2288)]
        public string PrivateKey { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        public bool WantsReport { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DaemonPreSharedKey> DaemonPreSharedKeys { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Daemon> Daemons { get; set; }

        public virtual LogedInUser LogedInUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
