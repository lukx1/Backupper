using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Daemon.Backups;
using System.IO;
using Shared.NetMessages.TaskMessages;
using System.Collections.Generic;
using System.Linq;
using Daemon.Logging;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class BackupTest
    {
        //private SmartBackup smartBackup;

        private void createTestFiles()
        {
            Directory.CreateDirectory(@"C:/TestBackupFolder");

        }

        private SmartBackup create(DbBackupType backupType, DbTaskDetails dbTaskDetails, IEnumerable<DbTaskLocation> taskLoc, int iID, string before, string after)
        {
            return new SmartBackup()
            {
                BackupType = backupType,
                ActionAfter = after,
                ActionBefore = before,
                ID = iID,
                TaskDetails = dbTaskDetails,
                TaskLocations = taskLoc
            };
        }

        [TestInitialize]
        public void Initialize()
        {
            UniLogger.CreateSourceInstance(null);
        }

        [TestMethod]
        public void BeforeAction()
        {
            const string testDir = "C:/BeforeActionDir";
            try { Directory.Delete(testDir); } catch (Exception) { }
            var b = create(null, null, Enumerable.Empty<DbTaskLocation>(), 1, $"mkdir \"{testDir}\"", null);
            b.StartBackup();
            Thread.Sleep(1000);
            Assert.IsTrue(Directory.Exists(testDir));
            try { Directory.Delete(testDir); } catch (Exception) { }
        }

        [TestMethod]
        public void AfterAction()
        {
            const string testDir = "C:/AfterActionDir";
            try { Directory.Delete(testDir); } catch (Exception) { }
            var b = create(null, null, Enumerable.Empty<DbTaskLocation>(), 1, null, $"mkdir \"{testDir}\"");
            b.StartBackup();
            Thread.Sleep(1000);
            Assert.IsTrue(Directory.Exists(testDir));
            try { Directory.Delete(testDir); } catch (Exception) { }
        }

        [TestMethod]
        public void Zip()
        {
            const string testFile = "C:/NormalTestFile";
            CreateTreeSourceDest(testFile);
            var bak = CreateLocBak(DbBackupType.NORM, testFile,"zip",0);
            bak.StartBackup();
            bak.StartBackup();
            var dirs = Directory.GetDirectories(Path.Combine(testFile, "Dest"));
            Assert.IsTrue(dirs.Length != 0, "Nebyla vytvořena žádná záloha");
        }


        [TestMethod]
        public void MySQL()
        {
            var testDir = "C:/SqlTest";

            if(Directory.Exists(testDir))
                Directory.Delete(testDir, true);

            Directory.CreateDirectory(testDir);

            var db = "3b1_joskalukas_db1";
            var host = "mysqlstudenti.litv.sssvt.cz"; ;
            var password = "123456";
            var username = "joskalukas";

            var bak = create(
                DbBackupType.NORM,
                new DbTaskDetails() { },
                CrtTaskLocE(
                    new DbTaskLocation()
                        {
                        destination = new DbLocation
                            {
                            protocol = DbProtocol.WND,
                            uri = Path.Combine(testDir,"Dest") },
                            source = new DbLocation {
                                protocol = DbProtocol.MYSQL,
                                uri = db,
                                LocationCredential = new DbLocationCredential()
                                    {
                                    LogonType = DbLogonType.Normal,
                                    host = host,
                                    password = password,
                                    username = username
                                }
                            }
                    }
                ),
                1,
                null,
                null);
            bak.StartBackup();
            var expectedResultFile = Path.Combine(testDir,"Dest", db + ".sql");
            Assert.IsTrue(File.Exists(expectedResultFile),"Soubor nebyl zkopírován");
            Assert.IsTrue(new FileInfo(expectedResultFile).Length > 0, "Zkopírovaný soubor je prázdný");
            //Directory.Delete(testDir, true);
        }

       

        private void CreateTreeSourceDest(string where)
        {
            string testDir = where;
            try { Directory.Delete(testDir, true); } catch (Exception) { }
            string source = Path.Combine(testDir, "Source");
            string dest = Path.Combine(testDir, "Dest");
            Directory.CreateDirectory(source);
            Directory.CreateDirectory(dest);
            File.WriteAllBytes(Path.Combine(source, "A.dum"), new byte[1024]);
            File.WriteAllBytes(Path.Combine(source, "B.dum"), new byte[1024]);
            File.WriteAllBytes(Path.Combine(source, "C.dum"), new byte[1024]);
            var sub = Path.Combine(source, "Sub");
            Directory.CreateDirectory(sub);
            File.WriteAllBytes(Path.Combine(sub, "D.dum"), new byte[1024]);
            File.WriteAllBytes(Path.Combine(sub, "E.dum"), new byte[1024]);
        }

        private IEnumerable<DbTaskLocation> CrtTaskLocE(params DbTaskLocation[] loc)
        {
            List<DbTaskLocation> list = new List<DbTaskLocation>();
            foreach (var l in loc)
            {
                list.Add(l);
            }
            return list;
        }

        private SmartBackup CreateLocBak(DbBackupType type, string testFile, string zip = null, int? level = null)
        {
            return create(DbBackupType.NORM, new DbTaskDetails() {CompressionLevel = level,ZipAlgorithm =zip }, CrtTaskLocE(
                new DbTaskLocation() { source = new DbLocation { protocol = DbProtocol.WND, uri = testFile + "/Source" }, destination = new DbLocation { protocol = DbProtocol.WND, uri = testFile + "/Dest" } }
                ), 1, null, null);
        }

        [TestMethod]
        public void Normal()
        {
            const string testFile = "C:/NormalTestFile";
            CreateTreeSourceDest(testFile);
            var bak = CreateLocBak(DbBackupType.NORM, testFile);
            bak.StartBackup();
            bak.StartBackup();
            var dirs = Directory.GetDirectories(Path.Combine(testFile,"Dest"));
            Assert.IsTrue(dirs.Length != 0, "Nebyla vytvořena žádná záloha");
            string[] sourceFiles = Directory.GetFiles(Path.Combine(testFile,"Source"), "*.*", SearchOption.AllDirectories).OrderBy(r => Path.GetFileName(r)).ToArray();

            for (int i = 0; i < sourceFiles.Length; i++)
            {
                string[] destFiles = new string[0];
                foreach (var dir in dirs)
                {
                    destFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories).OrderBy(r => Path.GetFileName(r)).ToArray();


                    Assert.IsTrue(sourceFiles.Length == destFiles.Length, "Počet kopírovaných souborů si není roven");
                    int y = 0;
                    foreach (var file in destFiles)
                    {
                        Assert.IsTrue(Path.GetFileName(sourceFiles[y]) == Path.GetFileName(file), "Soubor nebyl zkopírován");
                        Assert.IsTrue(new FileInfo(sourceFiles[y]).Length == new FileInfo(file).Length, "Kopírovaný soubor nemá stejnou velikost");
                        y++;
                    }

                }
            }
        }

        [TestMethod]
        public void Differentail()
        {
            const string testFile = "C:/DiffTestFile";
            try { Directory.Delete($@"C:\Users\{Environment.UserName}\AppData\Local\Backupper\Data", true); } catch (Exception) { }
            CreateTreeSourceDest(testFile);
            var bak = create(DbBackupType.DIFF, new DbTaskDetails() { }, CrtTaskLocE(
                new DbTaskLocation() { source = new DbLocation { protocol = DbProtocol.WND, uri = testFile + "/Source" }, destination = new DbLocation { protocol = DbProtocol.WND, uri = testFile + "/Dest" } }
                ), 1, null, null);
            bak.StartBackup();


            Thread.Sleep(1000);
            File.WriteAllText(Path.Combine(testFile, "Source", "A.dum"), "Updated");
            File.WriteAllText(Path.Combine(testFile, "Source", "X.dum"), "Created");

            bak.StartBackup();

            var dirs = Directory.GetDirectories(testFile + "/Dest");
            Assert.IsTrue(dirs.Length != 0, "Nebyla vytvořena žádná záloha");
            string[] sourceFiles = Directory.GetFiles(testFile + "/Source", "*.*", SearchOption.AllDirectories).OrderBy(r => Path.GetFileName(r)).ToArray();

            for (int i = 0; i < sourceFiles.Length; i++)
            {
                string[] destFiles = new string[0];
                if (i == 0)
                {
                    var dir = dirs[0];
                    destFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories).OrderBy(r => Path.GetFileName(r)).ToArray();

                    Assert.IsTrue(5 == destFiles.Length, "Počet kopírovaných souborů si není roven");
                    int y = 0;
                    foreach (var file in destFiles)
                    {
                        Assert.IsTrue(Path.GetFileName(sourceFiles[y]) == Path.GetFileName(file), "Soubor nebyl zkopírován");
                        //Assert.IsTrue(new FileInfo(sourceFiles[y]).Length == new FileInfo(file).Length, "Kopírovaný soubor nemá stejnou velikost");
                        y++;
                    }
                }
                else
                {
                    var dir = dirs[1];
                    Assert.IsTrue(Directory.GetFiles(dir).Length != 0, "Nic nebylo zkopírováno");
                    Assert.IsTrue(Directory.GetFiles(dir).Length == 2, "Byl zkopírován více než 1 soubor");
                }
            }
        }

        [TestMethod]
        public void Incremental()
        {
            const string testFile = "C:/IncTestFile";
            try { Directory.Delete($@"C:\Users\{Environment.UserName}\AppData\Local\Backupper\Data", true); } catch (Exception) { };
            CreateTreeSourceDest(testFile);
            var bak = create(DbBackupType.INCR, new DbTaskDetails() { }, CrtTaskLocE(
                new DbTaskLocation() { source = new DbLocation { protocol = DbProtocol.WND, uri = testFile + "/Source" }, destination = new DbLocation { protocol = DbProtocol.WND, uri = testFile + "/Dest" } }
                ), 1, null, null);
            bak.StartBackup();


            Thread.Sleep(1000);
            File.WriteAllText(Path.Combine(testFile, "Source", "A.dum"), "Updated");
            Thread.Sleep(1000);

            bak.StartBackup();

            var dirs = Directory.GetDirectories(testFile + "/Dest");
            Assert.IsTrue(dirs.Length != 0, "Nebyla vytvořena žádná záloha");
            string[] sourceFiles = Directory.GetFiles(testFile + "/Source", "*.*", SearchOption.AllDirectories).OrderBy(r => Path.GetFileName(r)).ToArray();

            for (int i = 0; i < sourceFiles.Length; i++)
            {
                string[] destFiles = new string[0];
                if (i == 0)
                {
                    var dir = dirs[0];
                    destFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories).OrderBy(r => Path.GetFileName(r)).ToArray();

                    Assert.IsTrue(sourceFiles.Length == destFiles.Length, "Počet kopírovaných souborů si není roven");
                    int y = 0;
                    foreach (var file in destFiles)
                    {
                        Assert.IsTrue(Path.GetFileName(sourceFiles[y]) == Path.GetFileName(file), "Soubor nebyl zkopírován");
                        //Assert.IsTrue(new FileInfo(sourceFiles[y]).Length == new FileInfo(file).Length, "Kopírovaný soubor nemá stejnou velikost");
                        y++;
                    }
                }
                else
                {
                    var dir = dirs[1];
                    Assert.IsTrue(Directory.GetFiles(dir).Length != 0, "Nic nebylo zkopírováno");
                    Assert.IsTrue(Directory.GetFiles(dir).Length == 1, "Byl zkopírován více než 1 soubor");
                }
            }
        }

    }
}
