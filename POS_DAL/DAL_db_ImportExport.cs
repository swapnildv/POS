using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Web;
using System.ComponentModel;
using MegabiteEntityLayer;

namespace POS_DAL
{

    public class DAL_db_ImportExport : Connection
    {


        //public System.Collections.IEnumerable GetAll_DBTables()
        //{
        //    string query = "select name from Cafeteria.sys.tables";

        //    string SqlConnStr = ConfigurationManager.ConnectionStrings["connCafeteria"].ConnectionString;

        //    var blogNames = dc.Database.SqlQuery<string>(query).ToList();
        //  //  blogNames = dc.ExecuteStoreQuery(query, DBNull.Value).ToList();
        //    // var blogs = dc.ExecuteStoreQuery(query).ToList();// SqlQuery("SELECT * FROM dbo.Blogs").ToList(); 

        //    //DataTable dt = new DataTable();
        //    //SqlConnection conn = new SqlConnection(SqlConnStr);
        //    //SqlCommand cmd = new SqlCommand(query, conn);
        //    //conn.Open();

        //    //// create data adapter
        //    //SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    //// this will query your database and return the result to your datatable
        //    //da.Fill(dt);
        //    //List<DataRow> list = dt.AsEnumerable().ToList();
        //    //conn.Close();
        //    //da.Dispose();
        //    return blogNames;
        //}

        public void ExportDataToXml(String TblName)
        {
            try
            {


                //String targetFolder = "C:\\CSV_files\\RemittancePayOutCSV";
                string SqlConnStr = ConfigurationManager.ConnectionStrings["connCafeteria"].ConnectionString;

                string CmdString = "select * from Employee_Master FOR XML RAW('Employee_Master'), ROOT('Root'), ELEMENTS";

                SqlConnection con;
                SqlCommand cmd;
                XmlReader reader;
                XmlDocument xmlDoc;

                using (con = new SqlConnection(SqlConnStr))
                {
                    cmd = new SqlCommand(CmdString, con);
                    con.Open();

                    reader = cmd.ExecuteXmlReader();
                    xmlDoc = new XmlDocument();

                    while (reader.Read())
                    {
                        xmlDoc.Load(reader);
                    }

                    xmlDoc.Save("C://" + TblName + ".xml");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
