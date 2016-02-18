using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hotel_POS.Resource
{
    public static class Log
    {
        private static object thisLock = new object();
        private static string logDirectory;

        static Log()
        {
            logDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            logDirectory += "\\Logs\\";

            if (Directory.Exists(logDirectory) == false)
                Directory.CreateDirectory(logDirectory);
        }

        public static void Write(string message)
        {
            lock (thisLock)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(new FileStream(logDirectory + "-" + DateTime.Now.ToString("yyyy-MMM-dd").ToUpper() + ".log", FileMode.Append)))
                    {
                        streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff").ToUpper() + "".PadLeft(4) + message);
                    }
                }
                catch { }
            }
        }

        public static void Write(Exception ex)
        {
            lock (thisLock)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(new FileStream(logDirectory + "-" + DateTime.Now.ToString("yyyy-MMM-dd").ToUpper() + ".log", FileMode.Append)))
                    {
                        streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff").ToUpper() + "".PadLeft(4) + ex.Message);
                        streamWriter.WriteLine(ex.StackTrace);
                        streamWriter.WriteLine(Environment.NewLine);
                        //streamWriter.WriteLine("___________________________________________________________________________________________________________________________");
                    }
                }
                catch { }
            }
        }



        /// <summary>
        /// Deletes all log files older than 7 days.
        /// </summary>
        public static void PurgeLog()
        {
            string[] updateFiles = Directory.GetFiles(logDirectory, "*.*", SearchOption.AllDirectories);
            foreach (string sourcePath in updateFiles)
            {
                string extension = System.IO.Path.GetExtension(sourcePath).ToLower().Trim();
                if (extension == ".log")
                {
                    String filename = System.IO.Path.GetFileName(sourcePath).ToLower();
                    filename = filename.Substring(0, (filename.Length - extension.Length));
                    DateTime dt = DateTime.Parse(filename);
                    dt = dt.AddDays(7);
                    if (dt < DateTime.Now)
                        File.Delete(sourcePath);
                }

            }
            Int16 s = Int16.Parse("h");

        }
    }

}
