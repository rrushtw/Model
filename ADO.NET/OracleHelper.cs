using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace ADO.NET
{
    public class OracleHelper
    {
        /* NOTCIE
         * In order to using this class, need to install Oracle.ManagedDataAccess.
         * There are two NuGet references, Oracle.ManagedDataAccess and Oracle.ManagedDataAccess.Core.
         * Please install one of them depend on target framework.
         */

        /// <summary>
        /// DataBase connection string example:
        /// "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=Host_IP)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=MyServiceName)));USER ID=MyID;PASSWORD=MyPassword;"
        /// </summary>
        private string DBConnectString { get; set; }

        public OracleHelper(string connectString)
        {
            DBConnectString = connectString;
        }

        /// <summary>
        /// Execute SQL command string
        /// </summary>
        /// <param name="commandString">SQL command string</param>
        /// <param name="parameters">Parameters in commanc string</param>
        /// <returns>DataTable</returns>
        public DataTable ExecSQL(string commandString, List<OracleParameter> parameters = null)
        {
            OracleConnection conn = new OracleConnection(DBConnectString);
            OracleCommand cmd = new OracleCommand(commandString, conn)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
            {
                foreach (OracleParameter x in parameters)
                {
                    if ((x.Direction == ParameterDirection.InputOutput || x.Direction == ParameterDirection.Input) && x.Value == null)
                    {
                        x.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(x);
                }
            }

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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Close the connection
                conn.Close();
            }

            return dt;
        }

        /// <summary>
        /// Execute SQL command string
        /// </summary>
        /// <param name="commandString">SQL command string</param>
        /// <param name="parameters">Parameters in command string</param>
        /// <returns>Affected rows</returns>
        public int GetAffectedRows(string commandString, List<OracleParameter> parameters = null)
        {
            int affectedRows = 0;
            OracleConnection conn = new OracleConnection(DBConnectString);
            OracleCommand cmd = new OracleCommand(commandString, conn)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
            {
                foreach (OracleParameter x in parameters)
                {
                    if ((x.Direction == ParameterDirection.InputOutput || x.Direction == ParameterDirection.Input) && x.Value == null)
                    {
                        x.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(x);
                }
            }

            try
            {
                //Open the connection
                conn.Open();
                //Execute
                affectedRows = cmd.ExecuteNonQuery();
                //Release cmd memories
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Close the connection
                conn.Close();
            }

            return affectedRows;
        }

        /// <summary>
        /// Execute stored procedure
        /// </summary>
        /// <param name="procudure">The name of stored procedure</param>
        /// <param name="parameters">Parameters that stored procedure required</param>
        /// <returns>DataTable</returns>
        public DataTable ExecProc(string procudure, List<OracleParameter> parameters = null)
        {
            OracleConnection conn = new OracleConnection(DBConnectString);
            OracleCommand cmd = new OracleCommand(procudure, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                foreach (OracleParameter x in parameters)
                {
                    if ((x.Direction == ParameterDirection.InputOutput || x.Direction == ParameterDirection.Input) && x.Value == null)
                    {
                        x.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(x);
                }
            }

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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Close the connection
                conn.Close();
            }

            return dt;
        }
    }
}
