using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene3DLib
{
    public enum LogLevel
    {
        ERR, WRN, INF
    }
    public class Logger
    {
        public static void inf(string s)
        {
            log(s, LogLevel.INF, false);
        }
        public static void wrn(string s)
        {
            log(s, LogLevel.WRN, false);
        }
        public static void err(string s)
        {
            log(s, LogLevel.ERR, false);
        }
        public static string getLogFN()
        {
            return Scene3D.AssemblyDirectory + "/log.log";
        }
        public static void log(string s, LogLevel l, bool debugWriteLine)
        {
            string logfn = getLogFN();
            using (StreamWriter sw = File.AppendText(logfn))
            {
                sw.WriteLine(l.ToString() + " " + s);
            }
        }
    }
}
