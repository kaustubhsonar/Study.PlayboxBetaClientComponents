using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace TestTool
{
    internal class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private const string UpdatedBy = "playbox1";//ToDo- read device id from config

        public void ExecuteStoredProcedureWithDataTableParameters(string spName,IList<TableParameter> parameters)
        {
            string connectionString = "";


            using (var conn = new SqlConnection(connectionString)) //ToDo-
            {
                conn.Open();

                var command = new SqlCommand
                {
                    CommandText = spName,
                    Connection = conn,
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter sqlParameter = command.Parameters.AddWithValue("@SenderHostname", UpdatedBy);
                sqlParameter.SqlDbType = SqlDbType.NVarChar;

                foreach (TableParameter parameter in parameters)
                {
                    sqlParameter = command.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    sqlParameter.SqlDbType = SqlDbType.Structured;
                }

                Execute(command);
            }
        }

        public void ExecuteStoredProcedureWithParameters(string spName, IList<SqlParameter> parameters)
        {
            string connectionString = "";


            using (var conn = new SqlConnection(connectionString)) //ToDo-
            {
                conn.Open();

                var command = new SqlCommand
                {
                    CommandText = spName,
                    Connection = conn,
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter sqlParameter = command.Parameters.AddWithValue("@SenderHostname", UpdatedBy);
                sqlParameter.SqlDbType = SqlDbType.NVarChar;

                foreach (SqlParameter parameter in parameters)
                {
                    //sqlParameter = command.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    //sqlParameter.SqlDbType = SqlDbType.Structured;
                }

                Execute(command);
            }
        }

        private static void Execute(SqlCommand command)
        {
            string spName = command.CommandText;
            try
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int errorNumber = (int)reader["ERRORNUMBER"];
                        string errorMessage = (string)reader["ERRORMESSAGE"];
                        if (reader.Read())
                        {
                            throw new SqlTypeException(
                                $"Unexpected no of error results from SP:{spName}. First:[errorNumber={errorNumber}, errorMessage={errorMessage}]");
                        }
                        throw new SqlTypeException(
                            $"Call to SP {spName} failed with errorNumber={errorNumber}, errorMessage={errorMessage}. parameters=");//ToDo-fill in Parameters
                    }
                }
            }
            catch (SqlException sqlex)
            {
                
                throw new SqlTypeException(
                    $"Call to SP {spName} failed in {command.Connection.ConnectionString} database with errorCode={sqlex.ErrorCode}, errorMessage={sqlex.Message}. parameters=",//ToDo-fill in Parameters
                    sqlex);
            }
        }
    }
}