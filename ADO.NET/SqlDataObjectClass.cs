using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MyModel
{
    class SqlObjectClass
    {
        private static string DBConnectString(string DataBase)
        {
            string[] Info = File.ReadAllLines(DataBase + ".txt");
            for(int i = 0; i < Info.Length; i++)
            {
                Info[i] = Info[i].Split(':')[1];
            }

            string ConnectString = "Data Source=" + Info[0] + ";Initial Catalog=" + Info[1] + ";Persist Security Info=True;User ID=" + Info[2] + ";Password=" + Info[3];

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
