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


namespace POS_DAL
{
    public class Connection
    {
        public MegabiteCafeteriaEntities dc = new MegabiteCafeteriaEntities();



        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public DataSet ExecuteDataset(SqlCommand command)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Conn_Cafeteria"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);


            DataSet dataset = new DataSet();
            SqlDataAdapter dataadapter;
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();

                command.Connection = connection;

                dataadapter = new SqlDataAdapter(command);

                dataadapter.Fill(dataset);
                connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return dataset;


        }


        public int ExecuteNonQueryCommand(SqlCommand command)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Conn_Cafeteria"].ConnectionString;
            SqlTransaction transaction;
            int rowAffected;
            SqlConnection connection = new SqlConnection(connectionString);


            DateTime dateSaveNow = DateTime.Now;
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
                connection.Open();
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                rowAffected = command.ExecuteNonQuery();
                transaction.Commit();

            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return rowAffected;

        }




    }
}




