using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src
{
    public class LogWriter
    {
        
            public static StringBuilder LogString = new StringBuilder();
        public static void WriteLine(string str)
        {
            Console.WriteLine(str);
            LogString.Append(str).Append(Environment.NewLine);
        }
        public static void Write(string str)
        {
            Console.Write(str);
            LogString.Append(str);

        }
        public static void SaveLog(bool Append = false, string Path = "./Log.txt")
        {
            if (LogString != null && LogString.Length > 0)
            {
                if (Append)
                {
                    using (StreamWriter file = System.IO.File.AppendText(Path))
                    {
                        file.Write(LogString.ToString());
                        file.Close();
                        file.Dispose();
                    }
                }
                else
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path))
                    {
                        file.Write(LogString.ToString());
                        file.Close();
                        file.Dispose();
                    }
                }
            }
            LogString = new StringBuilder();
        }
    }
    public class LogMessage
    {
        public static Task Log(Discord.LogMessage arg)
        {
            LogWriter.Write(DateTime.Now.ToString() + " | ");
            LogWriter.Write("[");
            switch (arg.Severity)
            {
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    LogWriter.Write("Debug");
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    LogWriter.Write("Verbose");
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    LogWriter.Write("Info");
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    LogWriter.Write("Warning");
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    LogWriter.Write("Error");
                    break;
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    LogWriter.Write("Critical");
                    break;


            }
            Console.ForegroundColor = ConsoleColor.White;
            LogWriter.Write("] | ");
            LogWriter.WriteLine(arg.Message);
            if (arg.Exception != null)
            {
                LogWriter.WriteLine(arg.Exception.Message);
                LogWriter.WriteLine(arg.Exception.StackTrace);
                LogWriter.WriteLine(arg.Exception.InnerException.ToString());
            }
            LogWriter.SaveLog(true, "log.txt");
            return Task.CompletedTask;
        }
    }
}

