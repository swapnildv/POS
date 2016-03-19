using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;
using log4net;

namespace Hotel_POS
{
    public class Common
    {
     
        public static string ServerName { get; set; }
        public static string Live_DBName { get; set; }
        public static string Demo_DBName { get; set; }
        public static string UserId { get; set; }
        public static string Password { get; set; }


        /// <summary>
        /// Initialise Server,Database Name,UserId,Password 
        /// </summary>
        public static void Initialise()
        {
            

            SqlConnectionStringBuilder Live_ConnBuilder = new SqlConnectionStringBuilder(Live_ConnString);
            ServerName = Live_ConnBuilder.DataSource;
            Live_DBName = Live_ConnBuilder.InitialCatalog;
            UserId = Live_ConnBuilder.UserID;
            Password = Live_ConnBuilder.Password;



            SqlConnectionStringBuilder Demo_ConnBuilder = new SqlConnectionStringBuilder(Demo_ConnString);
            ServerName = Demo_ConnBuilder.DataSource;
            Demo_DBName = Demo_ConnBuilder.InitialCatalog;




        }
        public static string Live_ConnString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connCafeteria"].ToString();
            }

        }
        public static string Demo_ConnString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["Demo_connCafeteria"].ToString();
            }

        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
    where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }

    public static class LoggingHelper
    {
        public static ILog GlobalLogger
        {
            get { return _globalLogger; }
        }
        private static ILog _globalLogger = log4net.LogManager.GetLogger("Global");
    }
}
