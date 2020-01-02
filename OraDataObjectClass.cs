using System;
using System.Data;
using System.IO;
using Oracle.ManagedDataAccess.Client;

namespace TG_DB.Model
{
    class OraDataObjectClass
    {
        private static string DBConnectString(string DataBase)
        {
            string[] Info = File.ReadAllLines(DataBase + ".txt");
            for(int i = 0; i < Info.Length; i++)
            {
                Info[i] = Info[i].Split(':')[1];
            }

            string ConnectString = "DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Info[0] 
                + ")(PORT=1521)))(CONNECT_DATA=(SID = " + Info[1] + ")));PERSIST SECURITY INFO=True;USER ID=" + Info[2] + ";PASSWORD=" + Info[3] + ";";

            return ConnectString;
        }

        public static DataTable LoadDataFromSQL(string DataBase, string CommandString)
        {
            OracleConnection conn = new OracleConnection(DBConnectString(DataBase));
            OracleCommand cmd = new OracleCommand(CommandString, conn);

            DataTable dt = new DataTable();
            try
            {
                //Open the connection
                conn.Open();
                //Load Data by each row
                OracleDataReader dr = cmd.ExecuteReader();
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
            OracleConnection conn = new OracleConnection(DBConnectString(DataBase));
            OracleCommand cmd = new OracleCommand(CommandString, conn);

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
