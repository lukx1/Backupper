namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MySQLContext : DbContext
    {
        private static MySQLContext mySQL;

        public MySQLContext(): base("name=MySQLContext")
        {
            
        }

        public virtual DbSet<BackupType> backupTypes { get; set; }
        public virtual DbSet<DaemonInfo> daemonInfos { get; set; }
        public virtual DbSet<DaemonPreSharedKey> daemonPreSharedKeys { get; set; }
        public virtual DbSet<Daemon> daemons { get; set; }
        public virtual DbSet<LocationCredential> locationCredentials { get; set; }
        public virtual DbSet<Location> locations { get; set; }
        public virtual DbSet<LogedInDaemon> logedInDaemons { get; set; }
        public virtual DbSet<LogonType> logonTypes { get; set; }
        public virtual DbSet<Protocol> protocols { get; set; }
        public virtual DbSet<TaskLocation> taskLocations { get; set; }
        public virtual DbSet<TaskLocationsTime> taskLocationsTimes { get; set; }
        public virtual DbSet<Task> tasks { get; set; }
        public virtual DbSet<Time> times { get; set; }
        public virtual DbSet<User> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackupType>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<BackupType>()
                .Property(e => e.LongName)
                .IsUnicode(false);

            modelBuilder.Entity<BackupType>()
                .HasMany(e => e.taskLocations)
                .WithRequired(e => e.backupType)
                .HasForeignKey(e => e.idBackupTypes)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Daemon>()
                .HasOptional(e => e.logedInDaemon)
                .WithRequired(e => e.daemon)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.tasks)
                .WithRequired(e => e.daemon)
                .HasForeignKey(e => e.idDaemon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocationCredential>()
                .Property(e => e.host)
                .IsUnicode(false);

            modelBuilder.Entity<LocationCredential>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<LocationCredential>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<LocationCredential>()
                .HasMany(e => e.locations)
                .WithRequired(e => e.locationCredential)
                .HasForeignKey(e => e.idLocationCredentails)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.uri)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.taskLocations)
                .WithRequired(e => e.location)
                .HasForeignKey(e => e.idSource)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.taskLocations1)
                .WithRequired(e => e.location1)
                .HasForeignKey(e => e.idDestination)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LogonType>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<LogonType>()
                .HasMany(e => e.locationCredentials)
                .WithRequired(e => e.logonType)
                .HasForeignKey(e => e.idLogonType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Protocol>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<Protocol>()
                .Property(e => e.LongName)
                .IsUnicode(false);

            modelBuilder.Entity<Protocol>()
                .HasMany(e => e.locations)
                .WithRequired(e => e.protocol)
                .HasForeignKey(e => e.idProtocol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskLocation>()
                .HasMany(e => e.taskLocationsTimes)
                .WithRequired(e => e.taskLocation)
                .HasForeignKey(e => e.idTaskLocation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.taskLocations)
                .WithRequired(e => e.task)
                .HasForeignKey(e => e.idTask)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Time>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Time>()
                .HasMany(e => e.taskLocationsTimes)
                .WithRequired(e => e.time)
                .HasForeignKey(e => e.idTime)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.daemonPreSharedKeys)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.idUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.daemons)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.idUser)
                .WillCascadeOnDelete(false);
        }
    }
}
