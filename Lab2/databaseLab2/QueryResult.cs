using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql.Schema;

namespace databaseLab2
{
    public class QueryResult
    {
        public bool QuerySuccess { get; private set; }
        public Exception QueryError { get; private set; }
        public List<string> ColumnData { get; private set; }
        public List<object[]> QueryData { get; private set; }
        public int RowsAffected { get; private set; }

        internal QueryResult(Exception ex)
        {
            QuerySuccess = false;
            QueryError = ex;
        }

        internal QueryResult(int rowsAffected)
        {
            QuerySuccess = true;
            RowsAffected = rowsAffected;
        }

        internal QueryResult(List<string> columnData, List<object[]> queryData)
        {
            QuerySuccess = true;
            ColumnData = columnData;
            QueryData = queryData;
        }

        private string objectToString(object obj)
        {
            if (obj == null || obj is DBNull)
                return "[null]";
            return obj.ToString();
        }

        public override string ToString()
        {
            if (QuerySuccess)
            {
                if (ColumnData == null)
                    return $"\n[SUCCESS]: Successful request. Rows affected: {RowsAffected}\n";
                else
                {
                    const int maxColSize = 30;
                    const string colSep = " | ";
                    const char colSepVert = '-';
                    const string colOverSizeString = "...";

                    var sb = new StringBuilder();
                    sb.AppendLine($"\n[SUCCSESS]: Successful request. Rows fetсhed: {QueryData.Count}\n");
                    var widths = new List<int>();
                    for (int i = 0; i < ColumnData.Count; i++)
                    {
                        var baseColumnName = ColumnData[i];
                        if (baseColumnName != null)
                        {
                            widths.Add(
                                QueryData
                                    .Select(p => objectToString(p[i]).Length)
                                    .Append(baseColumnName.Length)
                                    .Max());
                            if (widths[i] > maxColSize) widths[i] = maxColSize;
                            sb.Append(baseColumnName.PadRight(widths[i]));
                        }

                        sb.Append(colSep);
                    }
                    sb.AppendLine();
                    sb.AppendLine(new string(colSepVert, widths.Sum() + widths.Count * colSep.Length - 1));
                    foreach (var row in QueryData)
                    {
                        for (int j = 0; j < row.Length; j++)
                        {
                            var str = objectToString(row[j]);
                            str = str.Substring(0, Math.Min(str.Length, widths[j]));
                            sb.Append(str.PadRight(widths[j]));
                            sb.Append(colSep);
                        }
                        sb.AppendLine();
                    }
                    return sb.ToString();
                }
            }
            else return $"\n[FAILED]: Request processing failed with next exception.\n[FAILED]: {QueryError.Message}\n";
        }
    }
}