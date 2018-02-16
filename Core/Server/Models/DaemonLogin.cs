namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DaemonLogin : DbContext
    {
        public DaemonLogin()
            : base("name=DaemonLogin")
        {
        }

        public virtual DbSet<daemonInfo> daemonInfos { get; set; }
        public virtual DbSet<daemonPreSharedKey> daemonPreSharedKeys { get; set; }
        public virtual DbSet<daemon> daemons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<daemonInfo>()
                .Property(e => e.os)
                .IsUnicode(false);

            modelBuilder.Entity<daemonInfo>()
                .Property(e => e.mac)
                .IsUnicode(false);

            modelBuilder.Entity<daemonInfo>()
                .HasMany(e => e.daemons)
                .WithRequired(e => e.daemonInfo)
                .HasForeignKey(e => e.idDaemonInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<daemonPreSharedKey>()
                .Property(e => e.preSharedKey)
                .IsUnicode(false);

            modelBuilder.Entity<daemon>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
