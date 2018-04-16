using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupperDebugSoubory
{
    class Program
    {

        private static void Empty(System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        private static void CreateDummyFiles(string dest, int count, int bytes)
        {
            Directory.CreateDirectory(dest);
            for (int i = 0; i < count; i++)
            {
                File.WriteAllBytes(Path.Combine(dest, $"DummyFile{i}"), new byte[bytes]);
            }
        }

        static void Main(string[] args)
        {
            Empty(new DirectoryInfo(@"C:\BDEBUG"));

            List<DirectoryInfo> dirs = new List<DirectoryInfo>();
            dirs.AddRange(new DirectoryInfo[] {
                new DirectoryInfo(@"C:\BDEBUG\NORMAL\A"),
                new DirectoryInfo(@"C:\BDEBUG\NORMAL\B"),
                new DirectoryInfo(@"C:\BDEBUG\DIFF\A"),
                new DirectoryInfo(@"C:\BDEBUG\DIFF\B"),
                new DirectoryInfo(@"C:\BDEBUG\INCR\A"),
                new DirectoryInfo(@"C:\BDEBUG\INCR\B")
            });

            bool b = true;
            dirs.ForEach(r => {
                if(b)
                    CreateDummyFiles(r.FullName, 3, 1024);
                else
                    CreateDummyFiles(r.FullName, 0, 0);
                b = !b;
            });

        }
    }
}
