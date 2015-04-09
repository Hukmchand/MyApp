using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AntiTheftClub.Services
{
    public class SqlManager
    {
        public DataSet ExecuteStoredProcedure(string StoredProcedureName, params SqlParameter[] spParams)
        {
            return ExecuteQuery(StoredProcedureName, true, spParams);
        }

        private DataSet ExecuteQuery(string query, bool storedProcedure, params SqlParameter[] spParams)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlDataAdapter theDataAdapter;
            SqlConnection theConnection = new SqlConnection(connectionString);
            SqlCommand theCommand;
            DataSet theDataSet = new DataSet();

            try
            {
                theConnection.Open();

                theCommand = theConnection.CreateCommand();
                theCommand.Connection = theConnection;
                theCommand.CommandTimeout = 0;
                try
                {
                    if (storedProcedure)
                    {
                        theCommand.CommandType = CommandType.StoredProcedure;
                        theCommand.CommandText = query;
                    }
                    else
                    {
                        theCommand.CommandType = CommandType.Text;
                        theCommand.CommandText = query;
                    }
                    if (spParams != null)
                    {
                        foreach (SqlParameter spParam in spParams)
                        {
                            theCommand.Parameters.Add(spParam);
                        }
                    }
                    theDataAdapter = new SqlDataAdapter(theCommand);
                    theDataAdapter.Fill(theDataSet);
                    return theDataSet;
                }
                catch (SqlException sqlEx)
                {
                    throw (sqlEx);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (theConnection.State == ConnectionState.Open)
                {
                    theConnection.Close();
                }

            }
        }
    }
}