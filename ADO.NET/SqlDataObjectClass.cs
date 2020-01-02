using System;
using System.Data;
using System.Data.SqlClient;

namespace MyModel
{
    class SqlObjectClass
    {
        private static string DBConnectString(string DataBase)
        {
            string ConnectString = "Data Source=ip;Initial Catalog=DataBase;Persist Security Info=True;User ID=id;Password=pw";

            return ConnectString;
        }

        public static DataTable LoadDataFromSQL(string DataBase, string CommandString)
        {
            SqlConnection conn = new SqlConnection(DBConnectString(DataBase));
            SqlCommand cmd = new SqlCommand(CommandString, conn);

            DataTable dt = new DataTable();
            try
            {
                //Open the connection
                conn.Open();
                //Load Data by each row
                SqlDataReader dr = cmd.ExecuteReader();
                //Stored in data table
                dt.Load(dr);
                //Release cmd memories
                cmd.Dispose();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Close the connection
                conn.Close();
            }

            return dt;
        }

        public static void ExecSQL(string DataBase, string CommandString)
        {
            SqlConnection conn = new SqlConnection(DBConnectString(DataBase));
            SqlCommand cmd = new SqlCommand(CommandString, conn);

            try
            {
                //Open the connection
                conn.Open();
                //Execute
                cmd.ExecuteNonQuery();
                //Release cmd memories
                cmd.Dispose();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Close the connection
                conn.Close();
            }
        }
    }
}
