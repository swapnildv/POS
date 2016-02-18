using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using MegabiteEntityLayer;
using POS_DAL;

namespace POS_Business
{

    public class BL_db_ImportExport
    {
        DAL_db_ImportExport obj = new DAL_db_ImportExport();


        public System.Collections.IEnumerable GetAllTables()
        {
            return obj.GetAll_DBTables();
        }

        public void ExportRestDataToExel(String TableName)
        {
            obj.ExportDataToXml(TableName);
        }
    }
}
