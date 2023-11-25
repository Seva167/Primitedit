using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrimitierSaveEditor
{
    public static class Logger
    {
        private static string logPath = App.SettDir + "log.txt";

        static Logger()
        {
            File.Delete(logPath);
        }

        private static void LogInternal(string prefix, string msg)
        {
            File.AppendAllText(logPath, $"{DateTime.Now} [{prefix}] {msg}\n");
        }

        public static void LogInfo(object msg)
        {
            LogInternal("INFO", msg.ToString());
        }

        public static void LogWarning(object msg)
        {
            LogInternal("WARN", msg.ToString());
        }

        public static void LogError(object msg)
        {
            LogInternal("ERROR", msg.ToString());
        }

        public static void LogExc(object msg)
        {
            LogInternal("EXCEPTION", msg.ToString());
        }
    }
}
