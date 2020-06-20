using System.Data;
using System.Text;

namespace TestTool
{
    internal class TableParameter
    {
        public TableParameter(string name, DataTable value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public DataTable Value { get; }

        public override string ToString()
        {
            return $"{Name}:[{ToString(Value)}]";
        }

        private string ToString(DataTable dataTable)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    builder.Append(dataColumn.ColumnName);
                    builder.Append("=");
                    builder.Append(dataRow[dataColumn.ColumnName]);
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }
    }
}
