using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace TestTool
{
    //ToDo-Call StoredProcedureExecutor
    internal class DataPersister
    {
        private const string ConnectionString = @"Data Source=HarryComputer\PLAYBOXSQL; Integrated Security=false; Initial Catalog=Playbox; User Id=sa; Password=playbox";

        private readonly Guid deviceId = Guid.NewGuid();

        public DataPersister()
        {
        }
            
        public void PersistTrackInformation(string trackDetails, DateTime playedTime)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    if (connection.State != ConnectionState.Open) return;
                    var command = new SqlCommand
                    {
                        CommandText = "UpdateTrack",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };
                    
                    var sqlParameter = command.Parameters.AddWithValue("@DeviceHostId", deviceId);
                    sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;

                    sqlParameter = command.Parameters.AddWithValue("@TrackDetails", trackDetails);
                    sqlParameter.SqlDbType = SqlDbType.NVarChar;

                    sqlParameter = command.Parameters.AddWithValue("@PlayedTime", playedTime);//ToDo-Proper Rename
                    sqlParameter.SqlDbType = SqlDbType.DateTime2;

                    sqlParameter = command.Parameters.AddWithValue("@UpdatedBy", "Playbox1");
                    sqlParameter.SqlDbType = SqlDbType.NVarChar;

                    Execute(command);
                }
            }

            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
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
                        int errorNumber = (int) reader["ERRORNUMBER"];
                        string errorMessage = (string) reader["ERRORMESSAGE"];
                        if (reader.Read())
                        {
                            throw new SqlTypeException(
                                $"Unexpected no of error results from SP:{spName}. First:[errorNumber={errorNumber}, errorMessage={errorMessage}]");
                        }

                        throw new SqlTypeException(
                            $"Call to SP {spName} failed with errorNumber={errorNumber}, errorMessage={errorMessage}. parameters="); //ToDo-fill in Parameters
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new SqlTypeException(
                    $"Call to SP {spName} failed in {command.Connection.ConnectionString} database with errorCode={sqlException.ErrorCode}, errorMessage={sqlException.Message}. parameters=", //ToDo-fill in Parameters
                    sqlException);
            }
        }

    }
}
