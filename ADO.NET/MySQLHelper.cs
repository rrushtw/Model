using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ADO.NET
{
    public class MySQLHelper
    {
        private string DBConnectString { get; set; }

        public MySQLHelper(string connectString)
        {
            DBConnectString = connectString;
        }

        /// <summary>
        /// Execute SQL command string
        /// </summary>
        /// <param name="commandString">SQL command string</param>
        /// <param name="parameters">Parameters in commanc string</param>
        /// <returns>DataTable</returns>
        public DataTable ExecSQL(string commandString, IEnumerable<MySqlParameter> parameters = null)
        {
            MySqlConnection conn = new MySqlConnection(DBConnectString);
            MySqlCommand cmd = new MySqlCommand(commandString, conn)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
            {
                foreach (MySqlParameter x in parameters)
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
                MySqlDataReader dr = cmd.ExecuteReader();
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
        public int GetAffectedRows(string commandString, IEnumerable<MySqlParameter> parameters = null)
        {
            int affectedRows = 0;
            MySqlConnection conn = new MySqlConnection(DBConnectString);
            MySqlCommand cmd = new MySqlCommand(commandString, conn)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
            {
                foreach (MySqlParameter x in parameters)
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
        public DataTable ExecProc(string procudure, IEnumerable<MySqlParameter> parameters = null)
        {
            MySqlConnection conn = new MySqlConnection(DBConnectString);
            MySqlCommand cmd = new MySqlCommand(procudure, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                foreach (MySqlParameter x in parameters)
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
                MySqlDataReader dr = cmd.ExecuteReader();
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

        private T Mapping<T>(DataTable dt, DataRow dr) where T : class, new()
        {
            T model = new T();
            foreach (var prop in model.GetType().GetProperties())
            {
                if (!prop.CanWrite)
                {
                    continue;
                }

                if (dt.Columns.Contains(prop.Name))
                {
                    object propValue = null;
                    var value = dr[prop.Name];

                    if (value == DBNull.Value)
                    {
                        //propValue = null;
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        propValue = Convert.ToBoolean(value);
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        propValue = Convert.ToDateTime(value);
                    }
                    else if (prop.PropertyType == typeof(short))
                    {
                        propValue = Convert.ToInt16(value);
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        propValue = Convert.ToInt32(value);
                    }
                    else if (prop.PropertyType == typeof(long))
                    {
                        propValue = Convert.ToInt64(value);
                    }
                    else if (prop.PropertyType == typeof(decimal))
                    {
                        propValue = Convert.ToDecimal(value);
                    }
                    else
                    {
                        propValue = value;
                    }

                    prop.SetValue(model, propValue, null);
                }
            }

            return model;
        }

        public IEnumerable<T> ExecSQL<T>(string commandString, IEnumerable<MySqlParameter> parameters = null) where T : class, new()
        {
            DataTable dt = ExecSQL(commandString, parameters);

            if (dt == null)
            {
                throw new ArgumentNullException("DataTable is null!");
            }

            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T model = Mapping<T>(dt, row);
                list.Add(model);
            }
            return list;
        }

        public IEnumerable<T> ExecProc<T>(string procedure, IEnumerable<MySqlParameter> parameters = null) where T : class, new()
        {
            DataTable dt = ExecProc(procedure, parameters);

            if (dt == null)
            {
                throw new ArgumentNullException("DataTable is null!");
            }

            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T model = Mapping<T>(dt, row);
                list.Add(model);
            }

            return list;
        }
    }
}
