namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DaemonLoginContext : DbContext
    {
        public DaemonLoginContext()
            : base("name=DaemonLogin") // Neměnit je spojeno s Web.config
        {
        }

        public virtual DbSet<DaemonInfo> daemonInfos { get; set; }
        public virtual DbSet<DaemonPreSharedKey> daemonPreSharedKeys { get; set; }
        public virtual DbSet<Daemon> daemons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DaemonInfo>()
                .Property(e => e.os)
                .IsUnicode(false);

            modelBuilder.Entity<DaemonInfo>()
                .Property(e => e.mac)
                .IsUnicode(false);

            modelBuilder.Entity<DaemonInfo>()
                .HasMany(e => e.daemons)
                .WithRequired(e => e.daemonInfo)
                .HasForeignKey(e => e.idDaemonInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DaemonPreSharedKey>()
                .Property(e => e.preSharedKey)
                .IsUnicode(false);

            modelBuilder.Entity<Daemon>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
