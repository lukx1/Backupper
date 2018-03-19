using System;
using System.IO;

namespace Daemon.Utility
{
    /// <summary>
    /// Logger to filepath textfile
    /// </summary>
    public class Logger
    {
        private string path {get;set;}
        private string file { get; set; }
        private StreamWriter writer {get;set;}
        public bool Active = true;
        
        public Logger(string Path)
        {
           path = Path;
           writer = new StreamWriter(Path);
           writer.AutoFlush = true;
        }
        
        public void ErrorLog(string message)
        {
            log(GetTime() + "{Error}: " + message);
        }

        public void WarningLog(string message)
        {
            log(GetTime() + "{Warning}: " + message);
        }
        
        public void InfoLog(string message)
        {    
            log(GetTime() + "{Info}: " + message);
        }

        public void Log(string message)
        {
            log(GetTime() + message);
        }

        private void log(string message)
        {
            if(Active)
                writer.Write(message);
        }

        private string GetTime()
        {
            DateTime temp = DateTime.Now;
            return $"[{temp.Hour}:{temp.Minute}:{temp.Second}] ";
        }
    }
 }           