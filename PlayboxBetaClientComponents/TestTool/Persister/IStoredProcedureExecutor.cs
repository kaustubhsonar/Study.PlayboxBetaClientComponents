using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace TestTool
{
    internal interface IStoredProcedureExecutor
    {
        void ExecuteStoredProcedureWithDataTableParameters(string spName,IList<TableParameter> parameters);
    }
}
