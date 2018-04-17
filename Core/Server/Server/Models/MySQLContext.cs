namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MySQLContext : DbContext
    {
        public MySQLContext()
            : base(MySQLConnectionStringMaker.GetConnectionString())
        {
        }

        public virtual DbSet<BackupType> BackupTypes { get; set; }
        public virtual DbSet<DaemonGroup> DaemonGroups { get; set; }
        public virtual DbSet<DaemonInfo> DaemonInfos { get; set; }
        public virtual DbSet<DaemonPreSharedKey> DaemonPreSharedKeys { get; set; }
        public virtual DbSet<Daemon> Daemons { get; set; }
        public virtual DbSet<GroupPermission> GroupPermissions { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<LocationCredential> LocationCredentials { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LogedInDaemon> LogedInDaemons { get; set; }
        public virtual DbSet<LogedInUser> LogedInUsers { get; set; }
        public virtual DbSet<LogonType> LogonTypes { get; set; }
        public virtual DbSet<LogType> LogTypes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Protocol> Protocols { get; set; }
        public virtual DbSet<TaskDetail> TaskDetails { get; set; }
        public virtual DbSet<TaskLocation> TaskLocations { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskTime> TaskTimes { get; set; }
        public virtual DbSet<Time> Times { get; set; }
        public virtual DbSet<UniversalLog> UniversalLogs { get; set; }
        public virtual DbSet<UpTime> UpTimes { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackupType>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<BackupType>()
                .Property(e => e.LongName)
                .IsUnicode(false);

            modelBuilder.Entity<BackupType>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.BackupType)
                .HasForeignKey(e => e.IdBackupTypes);

            modelBuilder.Entity<DaemonInfo>()
                .Property(e => e.Os)
                .IsUnicode(false);

            modelBuilder.Entity<DaemonInfo>()
                .Property(e => e.Mac)
                .IsUnicode(false);

            modelBuilder.Entity<DaemonInfo>()
                .HasMany(e => e.Daemons)
                .WithRequired(e => e.DaemonInfo)
                .HasForeignKey(e => e.IdDaemonInfo);

            modelBuilder.Entity<DaemonPreSharedKey>()
                .Property(e => e.PreSharedKey)
                .IsUnicode(false);

            modelBuilder.Entity<Daemon>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.DaemonGroups)
                .WithRequired(e => e.Daemon)
                .HasForeignKey(e => e.IdDaemon);

            modelBuilder.Entity<Daemon>()
                .HasOptional(e => e.LogedInDaemon)
                .WithRequired(e => e.Daemon)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.Daemon)
                .HasForeignKey(e => e.IdDaemon);

            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.DaemonGroups)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.IdGroup);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupPermissions)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.IdGroup);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.UserGroups)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.IdGroup);

            modelBuilder.Entity<LocationCredential>()
                .Property(e => e.Host)
                .IsUnicode(false);

            modelBuilder.Entity<LocationCredential>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<LocationCredential>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<LocationCredential>()
                .HasMany(e => e.Locations)
                .WithOptional(e => e.LocationCredential)
                .HasForeignKey(e => e.IdLocationCredentails)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Location>()
                .Property(e => e.Uri)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.TaskLocations)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.IdDestination);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.TaskLocations1)
                .WithRequired(e => e.Location1)
                .HasForeignKey(e => e.IdSource);

            modelBuilder.Entity<LogonType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LogonType>()
                .HasMany(e => e.LocationCredentials)
                .WithOptional(e => e.LogonType)
                .HasForeignKey(e => e.IdLogonType)
                .WillCascadeOnDelete();

            modelBuilder.Entity<LogType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LogType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.GroupPermissions)
                .WithRequired(e => e.Permission)
                .HasForeignKey(e => e.IdPermission);

            modelBuilder.Entity<Protocol>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<Protocol>()
                .Property(e => e.LongName)
                .IsUnicode(false);

            modelBuilder.Entity<Protocol>()
                .HasMany(e => e.Locations)
                .WithRequired(e => e.Protocol)
                .HasForeignKey(e => e.IdProtocol);

            modelBuilder.Entity<TaskDetail>()
                .Property(e => e.ZipAlgorithm)
                .IsUnicode(false);

            modelBuilder.Entity<TaskDetail>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.TaskDetail)
                .HasForeignKey(e => e.IdTaskDetails);

            modelBuilder.Entity<Task>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.ActionBefore)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.ActionAfter)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.TaskLocations)
                .WithRequired(e => e.Task)
                .HasForeignKey(e => e.IdTask);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.TaskTimes)
                .WithRequired(e => e.Task)
                .HasForeignKey(e => e.IdTask);

            modelBuilder.Entity<Time>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Time>()
                .HasMany(e => e.TaskTimes)
                .WithRequired(e => e.Time)
                .HasForeignKey(e => e.IdTime);

            modelBuilder.Entity<UniversalLog>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Nickname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PublicKey)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PrivateKey)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DaemonPreSharedKeys)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Daemons)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.LogedInUser)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserGroups)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);
        }
    }
}
