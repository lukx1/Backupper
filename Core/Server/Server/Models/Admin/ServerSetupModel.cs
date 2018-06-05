using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using Shared;
using MySql.Data.MySqlClient;

namespace Server.Models.Admin
{
    /// <summary>
    /// Pro AdminSetup/Index
    /// </summary>
    public class ServerSetupModel
    {
        public string ConnectionString { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool WantsReport { get; set; }

        /// <summary>
        /// Otestuje jestli se jde připojit poskytnutým connection stringem k MySql serveru
        /// Pokud ano vytvoří admin uživatele dle zadaných parametrů
        /// </summary>
        public void Save()
        {
            try
            {
                var conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
                conn.Open();
            }
            catch(MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        throw new Exception("Cannot connect to server.");
                    case 1045:
                        throw new Exception("Invalid user/password.");
                }
            }

            Objects.ConnectionStringHelper.ConnectionString = ConnectionString;

            var admin = new User();
            admin.Nickname = Username;
            admin.FullName = Fullname;
            admin.Password = Password;
            admin.Email = Email;
            admin.WantsReport = WantsReport;
            string RSAPair = PasswordFactory.CreateRSAPrivateKey();
            admin.PrivateKey = PasswordFactory.EncryptAES(RSAPair, admin.Password);
            admin.Password = PasswordFactory.HashPasswordPbkdf2(admin.Password);
            admin.PublicKey = PasswordFactory.GetPublicFromRSAKeyPair(RSAPair);

            using (var db = new Models.MySQLContext(ConnectionString))
            {
                db.Users.Add(admin);
                db.SaveChanges();

                var dbAdmin = db.Users.FirstOrDefault(x => x.Nickname == admin.Nickname);
                dbAdmin.UserGroups.Add(new UserGroup() { IdUser = dbAdmin.Id, IdGroup = db.Groups.FirstOrDefault(x => x.Name == "Admins").Id });
                dbAdmin.UserGroups.Add(new UserGroup() { IdUser = dbAdmin.Id, IdGroup = db.Groups.FirstOrDefault(x => x.Name == "Users").Id });
                db.SaveChanges();
            }
        }
    }
}