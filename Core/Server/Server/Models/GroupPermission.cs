namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.GroupPermissions")]
    public partial class GroupPermission
    {
        public int Id { get; set; }

        public int IdGroup { get; set; }

        public int IdPermission { get; set; }

        public bool? Allow { get; set; }

        public bool? Deny { get; set; }

        public virtual Group Group { get; set; }

        public virtual Permission Permission { get; set; }

        public virtual Group Group1 { get; set; }

        public virtual Permission Permission1 { get; set; }
    }
}
