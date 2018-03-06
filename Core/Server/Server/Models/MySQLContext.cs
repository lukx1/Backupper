namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MySQLContext : DbContext
    {
        public MySQLContext()
            : base("name=MySQLContext")
        {
        }

        public virtual DbSet<BackupType> BackupTypes { get; set; }
        public virtual DbSet<DaemonGroup> DaemonGroups { get; set; }
        public virtual DbSet<DaemonInfo> DaemonInfos { get; set; }
        public virtual DbSet<DaemonLog> DaemonLogs { get; set; }
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
        public virtual DbSet<TaskLocationLog> TaskLocationLogs { get; set; }
        public virtual DbSet<TaskLocation> TaskLocations { get; set; }
        public virtual DbSet<TaskLocationsTime> TaskLocationsTimes { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Time> Times { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
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
                .HasMany(e => e.TaskLocations)
                .WithOptional(e => e.BackupType)
                .HasForeignKey(e => e.IdBackupTypes)
                .WillCascadeOnDelete();

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

            modelBuilder.Entity<DaemonLog>()
                .Property(e => e.ShortText)
                .IsUnicode(false);

            modelBuilder.Entity<DaemonLog>()
                .Property(e => e.LongText)
                .IsUnicode(false);

            modelBuilder.Entity<DaemonPreSharedKey>()
                .Property(e => e.PreSharedKey)
                .IsUnicode(false);

            modelBuilder.Entity<Daemon>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.DaemonGroups)
                .WithOptional(e => e.Daemon)
                .HasForeignKey(e => e.IdDaemon)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.DaemonLogs)
                .WithRequired(e => e.Daemon)
                .HasForeignKey(e => e.IdDaemon);

            modelBuilder.Entity<Daemon>()
                .HasOptional(e => e.LogedInDaemon)
                .WithRequired(e => e.Daemon)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.Daemon)
                .HasForeignKey(e => e.IdDaemon)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.DaemonGroups)
                .WithOptional(e => e.Group)
                .HasForeignKey(e => e.IdGroup)
                .WillCascadeOnDelete();

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
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.IdDestination)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Location>()
                .HasMany(e => e.TaskLocations1)
                .WithOptional(e => e.Location1)
                .HasForeignKey(e => e.IdSource)
                .WillCascadeOnDelete();

            modelBuilder.Entity<LogonType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LogType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LogType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<LogType>()
                .HasMany(e => e.LocationCredentials)
                .WithOptional(e => e.LogType)
                .HasForeignKey(e => e.IdLogonType)
                .WillCascadeOnDelete();

            modelBuilder.Entity<LogType>()
                .HasMany(e => e.TaskLocationLogs)
                .WithOptional(e => e.LogType)
                .HasForeignKey(e => e.IdLogType)
                .WillCascadeOnDelete();

            modelBuilder.Entity<LogType>()
                .HasMany(e => e.UserLogs)
                .WithRequired(e => e.LogType)
                .HasForeignKey(e => e.IdLogType);

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
                .WithOptional(e => e.Protocol)
                .HasForeignKey(e => e.IdProtocol)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TaskLocationLog>()
                .Property(e => e.ShortText)
                .IsUnicode(false);

            modelBuilder.Entity<TaskLocationLog>()
                .Property(e => e.LongText)
                .IsUnicode(false);

            modelBuilder.Entity<TaskLocation>()
                .HasMany(e => e.TaskLocationLogs)
                .WithOptional(e => e.TaskLocation)
                .HasForeignKey(e => e.IdTaskLocation)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TaskLocation>()
                .HasMany(e => e.TaskLocationsTimes)
                .WithOptional(e => e.TaskLocation)
                .HasForeignKey(e => e.IdTaskLocation)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Task>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.TaskLocations)
                .WithOptional(e => e.Task)
                .HasForeignKey(e => e.IdTask)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Time>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Time>()
                .HasMany(e => e.TaskLocationsTimes)
                .WithOptional(e => e.Time)
                .HasForeignKey(e => e.IdTime)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserLog>()
                .Property(e => e.ShortText)
                .IsUnicode(false);

            modelBuilder.Entity<UserLog>()
                .Property(e => e.LongText)
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

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserLogs)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);
        }
    }
}
