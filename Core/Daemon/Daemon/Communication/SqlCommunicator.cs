using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daemon.Communication
{
    public class SqlCommunicator
    {
        /// <summary>
        /// Vytvoří mysqldump
        /// </summary>
        /// <param name="Server">MySQL Server</param>
        /// <param name="Database">Databáze</param>
        /// <param name="UserID">Přihlašovací jméno</param>
        /// <param name="Password">Přihlašovací heslo</param>
        /// <param name="LocalDest">Zdrojoví lokální soubor</param>
        /// <param name="timeOut">Max délka pokusu o zálohu v ms</param>
        public async Task ImportFromFileAsync(string Server, string Database, string UserID, string Password, string LocalSource, int timeOut = 30000)
        {
            await Task.Run(() =>
            {
            using (MySqlConnection conn = new MySqlConnection($@"server={Server};persistsecurityinfo=True;database={Database};User ID={UserID};password={Password}"))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportCompleted += (o, e) => { Thread.CurrentThread.Interrupt(); };
                        mb.ImportFromFile(LocalSource);

                        try
                        {
                            Thread.Sleep(timeOut);
                        }
                        catch (ThreadInterruptedException) { }
                        conn.Close();
                    }
                }
            }
        });
        }

    /// <summary>
    /// Vytvoří mysqldump
    /// </summary>
    /// <param name="Server">MySQL Server</param>
    /// <param name="Database">Databáze</param>
    /// <param name="UserID">Přihlašovací jméno</param>
    /// <param name="Password">Přihlašovací heslo</param>
    /// <param name="LocalDest">Výstupní lokální soubour</param>
    /// <param name="timeOut">Max délka pokusu o zálohu v ms</param>
    public async Task ExportAsFileAsync(string Server, string Database, string UserID, string Password, string LocalDest, int timeOut = 30000)
        {
            await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection($@"server={Server};persistsecurityinfo=True;database={Database};User ID={UserID};password={Password}"))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportCompleted += (o, e) => { Thread.CurrentThread.Interrupt(); };
                            mb.ExportToFile(LocalDest);

                            try
                            {
                                Thread.Sleep(timeOut);
                            }
                            catch (ThreadInterruptedException) { }
                            conn.Close();
                        }
                    }
                }
            });
        }
    }
}
