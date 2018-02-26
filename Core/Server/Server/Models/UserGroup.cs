namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("3b1_joskalukas_db1.UserGroups")]
    public partial class UserGroup
    {
        public int Id { get; set; }

        public int IdUser { get; set; }

        public int IdGroup { get; set; }

        public virtual Group Group { get; set; }

        public virtual User User { get; set; }
    }
}
