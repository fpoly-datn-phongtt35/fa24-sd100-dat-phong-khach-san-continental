using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.StoredProcedure
{
    public class DbWorker
    {
        private static string _connection;
        public DbWorker(string connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get DataTable
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string procedureName, SqlParameter[] parameters = null)
        {
            DataTable _dataTable = new DataTable();
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
                    oCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        oCommand.Parameters.AddRange(parameters);
                    }

                    SqlDataAdapter oAdapter = new SqlDataAdapter();
                    oAdapter.SelectCommand = oCommand;
                    oConnection.Open();

                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oAdapter.SelectCommand.Transaction = oTransaction;
                            oAdapter.Fill(_dataTable);
                            oTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            string data_log = "";
                            if (parameters != null && parameters.Length > 0)
                            {
                                data_log = string.Join(",", parameters.Select(x => x.ParameterName)) + ":" + string.Join(",", parameters.Select(x => x.Value == null ? "NULL" : x.Value.ToString()));

                            }
                            /*LogHelper.InsertLogTelegram("SP Name: " + procedureName + "\n" + "Params: " + data_log + "\nGetDataTable - Transaction Rollback - DbWorker: " + ex);*/
                            oTransaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            if (oConnection.State == ConnectionState.Open)
                            {
                                oConnection.Close();
                            }
                            oConnection.Dispose();
                            oAdapter.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string data_log = "";
                if (parameters != null && parameters.Length > 0)
                {
                    data_log = string.Join(",", parameters.Select(x => x.ParameterName)) + ":" + string.Join(",", parameters.Select(x => x.Value == null ? "NULL" : x.Value.ToString()));

                }
               /* LogHelper.InsertLogTelegram("SP Name: " + procedureName + "\n" + "Params: " + data_log + "Error" + ex);*/
            }
            return _dataTable;
        }

        /// <summary>
        /// GET DataSet
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string procedureName, SqlParameter[] parameters = null)
        {
            DataSet _dataSet = new DataSet();
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
                    oCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        oCommand.Parameters.AddRange(parameters);
                    }
                    SqlDataAdapter oAdapter = new SqlDataAdapter();
                    oAdapter.SelectCommand = oCommand;
                    oConnection.Open();

                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oAdapter.SelectCommand.Transaction = oTransaction;
                            oAdapter.Fill(_dataSet);
                            oTransaction.Commit();
                            oCommand.Parameters.Clear();
                        }
                        catch (Exception ex)
                        {
                            string data_log = "";
                            if (parameters != null && parameters.Length > 0)
                            {
                                data_log = string.Join(",", parameters.Select(x => x.ParameterName)) + ":" + string.Join(",", parameters.Select(x => x.Value == null ? "NULL" : x.Value.ToString()));

                            }
                          /*  LogHelper.InsertLogTelegram("SP Name: " + procedureName + "\n" + "Params GetDataTable - Transaction Rollback - DbWorker: " + data_log + "\nGetDataSet - Transaction Rollback - DbWorker: " + ex);*/

                            oTransaction.Rollback();
                        }
                        finally
                        {
                            if (oConnection.State == ConnectionState.Open)
                            {
                                oConnection.Close();
                            }
                            oConnection.Dispose();
                            oAdapter.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                /*LogHelper.InsertLogTelegram("GetDataSet - DbWorker: " + ex);*/
            }
            return _dataSet;
        }

        public object ExecuteScalar(String procedureName, SqlParameter[] parameters = null)
        {
            object oReturnValue = null;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
                    oCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        oCommand.Parameters.AddRange(parameters);
                    }

                    oConnection.Open();

                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand.Transaction = oTransaction;
                            oReturnValue = oCommand.ExecuteScalar();
                            oTransaction.Commit();
                        }
                        catch
                        {
                            oTransaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            if (oConnection.State == ConnectionState.Open)
                            {
                                oConnection.Close();
                            }
                            oConnection.Dispose();
                            oCommand.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               /* LogHelper.InsertLogTelegram("ExecuteScalar - DbWorker: " + ex);*/
            }

            if (oReturnValue != null)
            {
                return oReturnValue;
            }
            else
            {
                return null;
            }
        }

        public int ExecuteNonQuery(string procedureName, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
                    oCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        oCommand.Parameters.AddRange(parameters);
                    }
                    int Rs = -1;
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {

                            oCommand.Transaction = oTransaction;
                            oCommand.ExecuteNonQuery();
                            oTransaction.Commit();
                            Rs = 1;
                        }
                        catch (Exception ex)
                        {
                            oTransaction.Rollback();
                            oCommand.Parameters.Clear();
                            return -1;
                        }
                        finally
                        {
                            oCommand.Parameters.Clear();
                            if (oConnection.State == ConnectionState.Open)
                            {
                                oConnection.Close();
                            }
                            oConnection.Dispose();
                            oCommand.Dispose();
                        }
                    }
                    return Rs;
                }
            }
            catch (Exception ex)
            {
               /* LogHelper.InsertLogTelegram("ExecuteNonQuery - DbWorker: " + ex);*/
                return -1;
            }
        }

        public void ExecuteNonQueryNoIdentity(string procedureName, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
                    oCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        oCommand.Parameters.AddRange(parameters);
                    }

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand.Transaction = oTransaction;
                            oCommand.ExecuteNonQuery();
                            oTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            string data_log = "";
                            if (parameters != null && parameters.Length > 0)
                            {
                                foreach (var param in parameters)
                                {
                                    data_log = string.Join(",", parameters.Select(x => x.ParameterName)) + ":" + string.Join(",", parameters.Select(x => x.Value == null ? "NULL" : x.Value.ToString()));
                                }
                            }
                           /* LogHelper.InsertLogTelegram("SP Name: " + procedureName + "\n" + "Params GetDataTable - Transaction Rollback - DbWorker: " + data_log + "\n ExecuteNonQueryNoIdentity - Transaction Rollback - DbWorker: " + ex);*/

                            oTransaction.Rollback();
                            oCommand.Parameters.Clear();
                        }
                        finally
                        {
                            oCommand.Parameters.Clear();
                            if (oConnection.State == ConnectionState.Open)
                            {
                                oConnection.Close();
                            }
                            oConnection.Dispose();
                            oCommand.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               /* LogHelper.InsertLogTelegram("ExecuteNonQueryNoIdentity - DbWorker: " + ex);*/
            }
        }

        public DataSet ExecuteSqlString(string SqlQuery)
        {
            DataSet _dataSet = new DataSet();
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    SqlCommand oCommand = new SqlCommand("execute_all_data", oConnection);
                    oCommand.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(SqlQuery))
                    {
                        oCommand.Parameters.AddWithValue("@SqlCommand", SqlQuery);
                    }
                    SqlDataAdapter oAdapter = new SqlDataAdapter();
                    oAdapter.SelectCommand = oCommand;
                    oConnection.Open();

                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oAdapter.SelectCommand.Transaction = oTransaction;
                            oAdapter.Fill(_dataSet);
                            oTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                           /* LogHelper.InsertLogTelegram("ExecuteSqlString - Transaction Rollback - DbWorker: " + ex);*/
                            oTransaction.Rollback();
                        }
                        finally
                        {
                            if (oConnection.State == ConnectionState.Open)
                            {
                                oConnection.Close();
                            }
                            oConnection.Dispose();
                            oAdapter.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // LogHelper.InsertLogTelegram("ExecuteScalar - DbWorker: " + ex);
            }
            return _dataSet;
        }

        public void ExecuteActionTransaction(Action<SqlConnection, SqlTransaction> act)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            act(oConnection, oTransaction);
                            oTransaction.Commit();
                        }
                        catch
                        {
                            oTransaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            if (oConnection.State == ConnectionState.Open) oConnection.Close();
                            oConnection.Dispose();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public T ExecuteFuncTransaction<T>(Func<SqlConnection, SqlTransaction, T> func)
        {
            try
            {
                T result;
                using (SqlConnection oConnection = new SqlConnection(_connection))
                {
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            result = func(oConnection, oTransaction);
                            oTransaction.Commit();
                        }
                        catch
                        {
                            oTransaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            if (oConnection.State == ConnectionState.Open) oConnection.Close();
                            oConnection.Dispose();
                        }
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
