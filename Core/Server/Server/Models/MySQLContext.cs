namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Shared;
    using System.Configuration;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Common;
    using System.Data.SqlClient;

    public partial class MySQLContext : DbContext
    {
        /*public MySQLContext() : base("name=MySQLContext")
        {
            Database.Connection.ConnectionString = Database.Connection.ConnectionString
                .Replace("[userid]", "joskalukas").Replace("[password]", "123456");
        }*/

        private static string CreateConnectionString()
        {
            string providerName = "MySql.Data.MySqlClient";
            string serverName = "mysqlstudenti.litv.sssvt.cz";
            string databaseName = "database=3b1_joskalukas_db1";

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.Add("Password", "123456");
            sqlBuilder.Add("User ID", "joskalukas");
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            //entityBuilder.Name = "MySQLContext";
            //Set the provider name.
            entityBuilder.Provider = providerName;
            
            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            //entityBuilder.Metadata = @"res://*/database=3b1_joskalukas_db1.csdl|
            //                res://*/database=3b1_joskalukas_db1.ssdl|
            //                res://*/database=3b1_joskalukas_db1.msl";

            return entityBuilder.ToString();
            //return "server=mysqlstudenti.litv.sssvt.cz;persistsecurityinfo=True;database=3b1_joskalukas_db1;User ID=joskalukas;password=123456";
        }


        public MySQLContext() : base("name=MySQLContext")
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
                .HasForeignKey(e => e.IdBackupTypes);

            modelBuilder.Entity<BackupType>()
                .HasMany(e => e.TaskLocations1)
                .WithOptional(e => e.BackupType1)
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

            modelBuilder.Entity<DaemonInfo>()
                .HasMany(e => e.Daemons1)
                .WithRequired(e => e.DaemonInfo1)
                .HasForeignKey(e => e.IdDaemonInfo)
                .WillCascadeOnDelete(false);

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
                .HasMany(e => e.DaemonGroups1)
                .WithOptional(e => e.Daemon1)
                .HasForeignKey(e => e.IdDaemon);

            modelBuilder.Entity<Daemon>()
                .HasOptional(e => e.LogedInDaemon)
                .WithRequired(e => e.Daemon)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Daemon>()
                .HasOptional(e => e.LogedInDaemon1)
                .WithRequired(e => e.Daemon1)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.Daemon)
                .HasForeignKey(e => e.IdDaemon)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Daemon>()
                .HasMany(e => e.Tasks1)
                .WithOptional(e => e.Daemon1)
                .HasForeignKey(e => e.IdDaemon);

            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.DaemonGroups)
                .WithOptional(e => e.Group)
                .HasForeignKey(e => e.IdGroup);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.DaemonGroups1)
                .WithOptional(e => e.Group1)
                .HasForeignKey(e => e.IdGroup);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupPermissions)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.IdGroup);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupPermissions1)
                .WithRequired(e => e.Group1)
                .HasForeignKey(e => e.IdGroup)
                .WillCascadeOnDelete(false);

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
                .HasForeignKey(e => e.IdLocationCredentails);

            modelBuilder.Entity<LocationCredential>()
                .HasMany(e => e.Locations1)
                .WithOptional(e => e.LocationCredential1)
                .HasForeignKey(e => e.IdLocationCredentails);

            modelBuilder.Entity<Location>()
                .Property(e => e.Uri)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.TaskLocations)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.IdSource);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.TaskLocations1)
                .WithOptional(e => e.Location1)
                .HasForeignKey(e => e.IdDestination);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.TaskLocations2)
                .WithOptional(e => e.Location2)
                .HasForeignKey(e => e.IdSource);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.TaskLocations3)
                .WithOptional(e => e.Location3)
                .HasForeignKey(e => e.IdDestination);

            modelBuilder.Entity<LogonType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LogonType>()
                .HasMany(e => e.LocationCredentials)
                .WithOptional(e => e.LogonType)
                .HasForeignKey(e => e.IdLogonType);

            modelBuilder.Entity<LogType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LogType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<LogType>()
                .HasMany(e => e.LocationCredentials)
                .WithOptional(e => e.LogType)
                .HasForeignKey(e => e.IdLogonType);

            modelBuilder.Entity<LogType>()
                .HasMany(e => e.TaskLocationLogs)
                .WithOptional(e => e.LogType)
                .HasForeignKey(e => e.IdLogType);

            modelBuilder.Entity<LogType>()
                .HasMany(e => e.UserLogs)
                .WithRequired(e => e.LogType)
                .HasForeignKey(e => e.IdLogType)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.GroupPermissions1)
                .WithRequired(e => e.Permission1)
                .HasForeignKey(e => e.IdPermission)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Protocol>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<Protocol>()
                .Property(e => e.LongName)
                .IsUnicode(false);

            modelBuilder.Entity<Protocol>()
                .HasMany(e => e.Locations)
                .WithOptional(e => e.Protocol)
                .HasForeignKey(e => e.IdProtocol);

            modelBuilder.Entity<Protocol>()
                .HasMany(e => e.Locations1)
                .WithOptional(e => e.Protocol1)
                .HasForeignKey(e => e.IdProtocol);

            modelBuilder.Entity<TaskLocationLog>()
                .Property(e => e.ShortText)
                .IsUnicode(false);

            modelBuilder.Entity<TaskLocationLog>()
                .Property(e => e.LongText)
                .IsUnicode(false);

            modelBuilder.Entity<TaskLocation>()
                .HasMany(e => e.TaskLocationLogs)
                .WithOptional(e => e.TaskLocation)
                .HasForeignKey(e => e.IdTaskLocation);

            modelBuilder.Entity<TaskLocation>()
                .HasMany(e => e.TaskLocationsTimes)
                .WithOptional(e => e.TaskLocation)
                .HasForeignKey(e => e.IdTaskLocation)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TaskLocation>()
                .HasMany(e => e.TaskLocationsTimes1)
                .WithOptional(e => e.TaskLocation1)
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

            modelBuilder.Entity<Task>()
                .HasMany(e => e.TaskLocations1)
                .WithOptional(e => e.Task1)
                .HasForeignKey(e => e.IdTask);

            modelBuilder.Entity<Time>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Time>()
                .HasMany(e => e.TaskLocationsTimes)
                .WithOptional(e => e.Time)
                .HasForeignKey(e => e.IdTime);

            modelBuilder.Entity<Time>()
                .HasMany(e => e.TaskLocationsTimes1)
                .WithOptional(e => e.Time1)
                .HasForeignKey(e => e.IdTime)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserLog>()
                .Property(e => e.ShortText)
                .IsUnicode(false);

            modelBuilder.Entity<UserLog>()
                .Property(e => e.LongText)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DaemonPreSharedKeys)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DaemonPreSharedKeys1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Daemons)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Daemons1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserGroups)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserLogs)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);
        }
    }
}